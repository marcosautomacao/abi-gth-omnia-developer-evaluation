using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Commands.CreateSale
{
    public class CreateSaleCommandHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly CreateSaleCommandHandler _handler;

        public CreateSaleCommandHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new CreateSaleCommandHandler(_saleRepository);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateSaleAndReturnId()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "SALE001",
                CustomerId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                BranchId = Guid.NewGuid(),
                BranchName = "Test Branch",
                Items = new List<CreateSaleItemCommand>
                {
                    new CreateSaleItemCommand
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Test Product",
                        Quantity = 5,
                        UnitPrice = 10.0m
                    }
                }
            };

            Sale? capturedSale = null;
            await _saleRepository.AddAsync(Arg.Do<Sale>(sale => capturedSale = sale));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>());
            
            result.Should().NotBeEmpty();
            capturedSale.Should().NotBeNull();
            capturedSale!.SaleNumber.Should().Be(command.SaleNumber);
            capturedSale.CustomerId.Should().Be(command.CustomerId);
            capturedSale.CustomerName.Should().Be(command.CustomerName);
            capturedSale.BranchId.Should().Be(command.BranchId);
            capturedSale.BranchName.Should().Be(command.BranchName);
            
            capturedSale.Items.Should().HaveCount(1);
            var item = capturedSale.Items.First();
            item.ProductId.Should().Be(command.Items[0].ProductId);
            item.ProductName.Should().Be(command.Items[0].ProductName);
            item.Quantity.Should().Be(command.Items[0].Quantity);
            item.UnitPrice.Should().Be(command.Items[0].UnitPrice);
            
            var domainEvent = capturedSale.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<SaleCreatedEvent>().Subject;
            domainEvent.SaleId.Should().Be(capturedSale.Id);
        }

        [Fact]
        public async Task Handle_WithMultipleItems_ShouldCreateSaleWithAllItems()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "SALE001",
                CustomerId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                BranchId = Guid.NewGuid(),
                BranchName = "Test Branch",
                Items = new List<CreateSaleItemCommand>
                {
                    new CreateSaleItemCommand
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 1",
                        Quantity = 5,
                        UnitPrice = 10.0m
                    },
                    new CreateSaleItemCommand
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 2",
                        Quantity = 3,
                        UnitPrice = 20.0m
                    }
                }
            };

            Sale? capturedSale = null;
            await _saleRepository.AddAsync(Arg.Do<Sale>(sale => capturedSale = sale));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>());
            
            capturedSale.Should().NotBeNull();
            capturedSale!.Items.Should().HaveCount(2);
            capturedSale.Items.First().ProductName.Should().Be("Product 1");
            capturedSale.Items.Skip(1).First().ProductName.Should().Be("Product 2");
        }

        [Fact]
        public async Task Handle_WithNoItems_ShouldCreateSaleWithEmptyItemsList()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "SALE001",
                CustomerId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                BranchId = Guid.NewGuid(),
                BranchName = "Test Branch",
                Items = new List<CreateSaleItemCommand>()
            };

            Sale? capturedSale = null;
            await _saleRepository.AddAsync(Arg.Do<Sale>(sale => capturedSale = sale));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>());
            capturedSale.Should().NotBeNull();
            capturedSale!.Items.Should().BeEmpty();
        }
    }
} 