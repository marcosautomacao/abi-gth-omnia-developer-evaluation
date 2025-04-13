using System;
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateSaleItem()
        {
            // Arrange
            var sale = CreateValidSale();
            var productId = Guid.NewGuid();
            var productName = "Test Product";
            var quantity = 5;
            var unitPrice = 10.0m;

            // Act
            var saleItem = new SaleItem(sale, productId, productName, quantity, unitPrice);

            // Assert
            saleItem.Sale.Should().Be(sale);
            saleItem.ProductId.Should().Be(productId);
            saleItem.ProductName.Should().Be(productName);
            saleItem.Quantity.Should().Be(quantity);
            saleItem.UnitPrice.Should().Be(unitPrice);
            saleItem.TotalAmount.Should().Be(45.0m); // 5 * 10 * 0.9 (10% discount)
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithInvalidQuantity_ShouldThrowException(int invalidQuantity)
        {
            // Arrange
            var sale = CreateValidSale();
            var productId = Guid.NewGuid();
            var productName = "Test Product";
            var unitPrice = 10.0m;

            // Act & Assert
            var action = () => new SaleItem(sale, productId, productName, invalidQuantity, unitPrice);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Quantity must be greater than zero (Parameter 'quantity')");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithInvalidUnitPrice_ShouldThrowException(decimal invalidUnitPrice)
        {
            // Arrange
            var sale = CreateValidSale();
            var productId = Guid.NewGuid();
            var productName = "Test Product";
            var quantity = 5;

            // Act & Assert
            var action = () => new SaleItem(sale, productId, productName, quantity, invalidUnitPrice);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Unit price must be greater than zero (Parameter 'unitPrice')");
        }

        [Fact]
        public void UpdateQuantity_WithValidQuantity_ShouldUpdateQuantityAndTotal()
        {
            // Arrange
            var saleItem = CreateValidSaleItem();
            var newQuantity = 10;

            // Act
            saleItem.UpdateQuantity(newQuantity);

            // Assert
            saleItem.Quantity.Should().Be(newQuantity);
            saleItem.TotalAmount.Should().Be(80.0m); // 10 * 10 * 0.8 (20% discount)
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(21)]
        public void UpdateQuantity_WithInvalidQuantity_ShouldThrowException(int invalidQuantity)
        {
            // Arrange
            var saleItem = CreateValidSaleItem();

            // Act & Assert
            var action = () => saleItem.UpdateQuantity(invalidQuantity);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void UpdateUnitPrice_WithValidPrice_ShouldUpdatePriceAndTotal()
        {
            // Arrange
            var saleItem = CreateValidSaleItem();
            var newUnitPrice = 20.0m;

            // Act
            saleItem.UpdateUnitPrice(newUnitPrice);

            // Assert
            saleItem.UnitPrice.Should().Be(newUnitPrice);
            saleItem.TotalAmount.Should().Be(90.0m); // 5 * 20 * 0.9 (10% discount)
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void UpdateUnitPrice_WithInvalidPrice_ShouldThrowException(decimal invalidPrice)
        {
            // Arrange
            var saleItem = CreateValidSaleItem();

            // Act & Assert
            var action = () => saleItem.UpdateUnitPrice(invalidPrice);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Unit price must be greater than zero (Parameter 'newUnitPrice')");
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

        private static SaleItem CreateValidSaleItem()
        {
            var sale = CreateValidSale();
            return new SaleItem(
                sale,
                Guid.NewGuid(),
                "Test Product",
                5,
                10.0m);
        }
    }
} 