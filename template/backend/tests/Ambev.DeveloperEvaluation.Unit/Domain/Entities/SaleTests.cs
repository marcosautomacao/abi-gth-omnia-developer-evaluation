using System;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateSale()
        {
            // Arrange
            var saleNumber = "SALE001";
            var customerId = Guid.NewGuid();
            var customerName = "Test Customer";
            var branchId = Guid.NewGuid();
            var branchName = "Test Branch";

            // Act
            var sale = new Sale(saleNumber, customerId, customerName, branchId, branchName);

            // Assert
            sale.SaleNumber.Should().Be(saleNumber);
            sale.CustomerId.Should().Be(customerId);
            sale.CustomerName.Should().Be(customerName);
            sale.BranchId.Should().Be(branchId);
            sale.BranchName.Should().Be(branchName);
            sale.SaleDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            sale.IsCancelled.Should().BeFalse();
            sale.TotalAmount.Should().Be(0);
            sale.Items.Should().BeEmpty();
            sale.DomainEvents.Should().ContainSingle(e => e is SaleCreatedEvent);
        }

        [Fact]
        public void AddItem_WithValidParameters_ShouldAddItemAndUpdateTotal()
        {
            // Arrange
            var sale = CreateValidSale();
            var productId = Guid.NewGuid();
            var productName = "Test Product";
            var quantity = 5;
            var unitPrice = 10.0m;

            // Act
            sale.AddItem(productId, productName, quantity, unitPrice);

            // Assert
            sale.Items.Should().HaveCount(1);
            var item = sale.Items.Should().ContainSingle().Subject;
            item.ProductId.Should().Be(productId);
            item.ProductName.Should().Be(productName);
            item.Quantity.Should().Be(quantity);
            item.UnitPrice.Should().Be(unitPrice);
            // 5 items at $10 each with 10% discount = $45
            sale.TotalAmount.Should().Be(45.0m);
        }

        [Fact]
        public void Cancel_WhenNotCancelled_ShouldCancelSale()
        {
            // Arrange
            var sale = CreateValidSale();

            // Act
            sale.Cancel();

            // Assert
            sale.IsCancelled.Should().BeTrue();
            sale.DomainEvents.Should().Contain(e => e is SaleCancelledEvent);
        }

        [Fact]
        public void Cancel_WhenAlreadyCancelled_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();
            sale.Cancel();

            // Act & Assert
            var action = () => sale.Cancel();
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("Sale is already cancelled");
        }

        [Fact]
        public void AddItem_WithMultipleItems_ShouldCalculateTotalCorrectly()
        {
            // Arrange
            var sale = CreateValidSale();

            // Act
            sale.AddItem(Guid.NewGuid(), "Product 1", 3, 10.0m); // No discount
            sale.AddItem(Guid.NewGuid(), "Product 2", 5, 10.0m); // 10% discount
            sale.AddItem(Guid.NewGuid(), "Product 3", 15, 10.0m); // 20% discount

            // Assert
            // Product 1: 3 * $10 = $30
            // Product 2: 5 * $10 * 0.9 = $45
            // Product 3: 15 * $10 * 0.8 = $120
            // Total: $195
            sale.TotalAmount.Should().Be(195.0m);
            sale.Items.Should().HaveCount(3);
        }

        private static Sale CreateValidSale()
        {
            return new Sale(
                "SALE001",
                Guid.NewGuid(),
                "Test Customer",
                Guid.NewGuid(),
                "Test Branch");
        }
    }
} 