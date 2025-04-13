using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.Dtos;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Queries.GetSaleById
{
    public class GetSaleByIdQueryHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetSaleByIdQueryHandler _handler;

        public GetSaleByIdQueryHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetSaleByIdQueryHandler(_saleRepository, _mapper);
        }

        [Fact]
        public async Task Handle_WithExistingSale_ShouldReturnMappedSaleDto()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var query = new GetSaleByIdQuery { Id = saleId };
            
            var sale = new Sale("SALE001", Guid.NewGuid(), "Test Customer", Guid.NewGuid(), "Test Branch");
            var expectedDto = new SaleDto
            {
                Id = saleId,
                SaleNumber = "SALE001",
                CustomerName = "Test Customer",
                BranchName = "Test Branch"
            };

            _saleRepository.GetByIdAsync(saleId).Returns(sale);
            _mapper.Map<SaleDto>(sale).Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).GetByIdAsync(saleId);
            _mapper.Received(1).Map<SaleDto>(sale);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task Handle_WithNonExistingSale_ShouldReturnNull()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var query = new GetSaleByIdQuery { Id = saleId };

            _saleRepository.GetByIdAsync(saleId).Returns((Sale?)null);
            _mapper.Map<SaleDto>(null).Returns((SaleDto?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).GetByIdAsync(saleId);
            _mapper.Received(1).Map<SaleDto>(null);
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_WithSaleAndItems_ShouldReturnDtoWithMappedItems()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var query = new GetSaleByIdQuery { Id = saleId };
            
            var sale = new Sale("SALE001", Guid.NewGuid(), "Test Customer", Guid.NewGuid(), "Test Branch");
            sale.AddItem(Guid.NewGuid(), "Test Product", 5, 10.0m);

            var expectedDto = new SaleDto
            {
                Id = saleId,
                SaleNumber = "SALE001",
                CustomerName = "Test Customer",
                BranchName = "Test Branch",
                Items = new[]
                {
                    new SaleItemDto
                    {
                        ProductName = "Test Product",
                        Quantity = 5,
                        UnitPrice = 10.0m,
                        TotalAmount = 45.0m // 5 * 10 * 0.9 (10% discount)
                    }
                }
            };

            _saleRepository.GetByIdAsync(saleId).Returns(sale);
            _mapper.Map<SaleDto>(sale).Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).GetByIdAsync(saleId);
            _mapper.Received(1).Map<SaleDto>(sale);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDto);
            result.Items.Should().HaveCount(1);
            result.Items.First().TotalAmount.Should().Be(45.0m);
        }
    }
} 