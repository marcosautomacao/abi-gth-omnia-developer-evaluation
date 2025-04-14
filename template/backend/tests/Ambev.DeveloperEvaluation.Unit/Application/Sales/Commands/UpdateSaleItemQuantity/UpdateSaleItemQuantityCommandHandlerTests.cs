using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSaleItemQuantity;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Commands.UpdateSaleItemQuantity
{
    public class UpdateSaleItemQuantityCommandHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly UpdateSaleItemQuantityCommandHandler _handler;
        private readonly string _saleNumber = "SALE001";
        private readonly Guid _productId = Guid.NewGuid();

        public UpdateSaleItemQuantityCommandHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new UpdateSaleItemQuantityCommandHandler(_saleRepository);
        }

        [Fact]
        public async Task Handle_WithValidRequest_ShouldUpdateQuantityAndRaiseEvent()
        {
            // Arrange
            var command = new UpdateSaleItemQuantityCommand 
            { 
                SaleNumber = _saleNumber,
                ProductId = _productId,
                NewQuantity = 10
            };

            var sale = new Sale(_saleNumber, Guid.NewGuid(), "Test Customer", Guid.NewGuid(), "Test Branch");
            sale.AddItem(_productId, "Test Product", 5, 10.0m);
            sale.ClearDomainEvents(); // Clear any events from creation and adding item
            
            _saleRepository.GetBySaleNumberAsync(_saleNumber).Returns(sale);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).GetBySaleNumberAsync(_saleNumber);
            await _saleRepository.Received(1).UpdateAsync(sale);
            
            var item = sale.Items.Should().ContainSingle().Subject;
            item.Quantity.Should().Be(10);
            item.TotalAmount.Should().Be(80.0m); // 10 * 10 * 0.8 (20% discount for 10+ items)
            
            var domainEvent = item.DomainEvents.Should().ContainSingle().Subject;
            domainEvent.Should().BeOfType<SaleItemModifiedEvent>();
            ((SaleItemModifiedEvent)domainEvent).ItemId.Should().Be(item.Id);
            ((SaleItemModifiedEvent)domainEvent).SaleId.Should().Be(sale.Id);
        }

        [Fact]
        public async Task Handle_WithNonExistingSale_ShouldThrowException()
        {
            // Arrange
            var command = new UpdateSaleItemQuantityCommand 
            { 
                SaleNumber = _saleNumber,
                ProductId = _productId,
                NewQuantity = 10
            };

            _saleRepository.GetBySaleNumberAsync(_saleNumber).Returns((Sale?)null);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Sale with number {_saleNumber} not found");
            await _saleRepository.Received(1).GetBySaleNumberAsync(_saleNumber);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
        }

        [Fact]
        public async Task Handle_WithNonExistingProduct_ShouldThrowException()
        {
            // Arrange
            var command = new UpdateSaleItemQuantityCommand 
            { 
                SaleNumber = _saleNumber,
                ProductId = _productId,
                NewQuantity = 10
            };

            var sale = new Sale(_saleNumber, Guid.NewGuid(), "Test Customer", Guid.NewGuid(), "Test Branch");
            sale.AddItem(Guid.NewGuid(), "Different Product", 5, 10.0m); // Add different product
            sale.ClearDomainEvents(); // Clear any events from creation and adding item
            
            _saleRepository.GetBySaleNumberAsync(_saleNumber).Returns(sale);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Product {_productId} not found in sale {_saleNumber}");
            await _saleRepository.Received(1).GetBySaleNumberAsync(_saleNumber);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(21)]
        public async Task Handle_WithInvalidQuantity_ShouldThrowException(int invalidQuantity)
        {
            // Arrange
            var command = new UpdateSaleItemQuantityCommand 
            { 
                SaleNumber = _saleNumber,
                ProductId = _productId,
                NewQuantity = invalidQuantity
            };

            var sale = new Sale(_saleNumber, Guid.NewGuid(), "Test Customer", Guid.NewGuid(), "Test Branch");
            sale.AddItem(_productId, "Test Product", 5, 10.0m);
            sale.ClearDomainEvents(); // Clear any events from creation and adding item
            
            _saleRepository.GetBySaleNumberAsync(_saleNumber).Returns(sale);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
            await _saleRepository.Received(1).GetBySaleNumberAsync(_saleNumber);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
        }

        [Fact]
        public async Task Handle_WithCancelledSale_ShouldThrowException()
        {
            // Arrange
            var command = new UpdateSaleItemQuantityCommand 
            { 
                SaleNumber = _saleNumber,
                ProductId = _productId,
                NewQuantity = 10
            };

            var sale = new Sale(_saleNumber, Guid.NewGuid(), "Test Customer", Guid.NewGuid(), "Test Branch");
            sale.AddItem(_productId, "Test Product", 5, 10.0m);
            sale.ClearDomainEvents(); // Clear any events from creation and adding item
            sale.Cancel(); // Cancel the sale
            
            _saleRepository.GetBySaleNumberAsync(_saleNumber).Returns(sale);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Cannot update items in a cancelled sale");
            await _saleRepository.Received(1).GetBySaleNumberAsync(_saleNumber);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
        }
    }
} 