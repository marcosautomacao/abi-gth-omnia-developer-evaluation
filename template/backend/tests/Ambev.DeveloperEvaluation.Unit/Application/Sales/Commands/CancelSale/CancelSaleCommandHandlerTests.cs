using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Commands.CancelSale
{
    public class CancelSaleCommandHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly CancelSaleCommandHandler _handler;

        public CancelSaleCommandHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new CancelSaleCommandHandler(_saleRepository);
        }

        [Fact]
public async Task Handle_WithValidSaleNumber_ShouldCancelSaleAndRaiseEvent()
{
    // Arrange
    var command = new CancelSaleCommand { SaleNumber = "SALE001" };
    var sale = new Sale("SALE001", Guid.NewGuid(), "Test Customer", Guid.NewGuid(), "Test Branch");
    
    _saleRepository.GetBySaleNumberAsync(command.SaleNumber).Returns(sale);

    // Act
    await _handler.Handle(command, CancellationToken.None);

    // Assert
    await _saleRepository.Received(1).GetBySaleNumberAsync(command.SaleNumber);
    await _saleRepository.Received(1).UpdateAsync(sale);
    
    sale.IsCancelled.Should().BeTrue();

    // Verify that the SaleCancelledEvent is present in the DomainEvents collection
    var domainEvent = sale.DomainEvents.Should().ContainSingle(e => e is SaleCancelledEvent).Which.As<SaleCancelledEvent>();
    domainEvent.SaleId.Should().Be(sale.Id);
}

        [Fact]
        public async Task Handle_WithNonExistingSaleNumber_ShouldThrowException()
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = "NONEXISTENT" };
            _saleRepository.GetBySaleNumberAsync(command.SaleNumber).Returns((Sale?)null);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Sale with number {command.SaleNumber} not found");
            await _saleRepository.Received(1).GetBySaleNumberAsync(command.SaleNumber);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
        }

        [Fact]
        public async Task Handle_WithAlreadyCancelledSale_ShouldThrowException()
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = "SALE001" };
            var sale = new Sale("SALE001", Guid.NewGuid(), "Test Customer", Guid.NewGuid(), "Test Branch");
            sale.Cancel(); // Cancel the sale first
            
            _saleRepository.GetBySaleNumberAsync(command.SaleNumber).Returns(sale);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Sale is already cancelled");
            await _saleRepository.Received(1).GetBySaleNumberAsync(command.SaleNumber);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
        }
    }
} 