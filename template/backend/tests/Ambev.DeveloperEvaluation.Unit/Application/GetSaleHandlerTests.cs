using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        var config = new MapperConfiguration(cfg => 
        {
            cfg.AddMaps(typeof(GetSaleProfile).Assembly);
        });
        _mapper = config.CreateMapper();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    [Theory]
    [InlineData(3, 0)] // Below 4 items - no discount
    [InlineData(4, 0.10)] // 4 items - 10% discount
    [InlineData(9, 0.10)] // Between 4-9 items - 10% discount
    [InlineData(10, 0.20)] // 10 items - 20% discount
    [InlineData(15, 0.20)] // Between 10-20 items - 20% discount
    [InlineData(20, 0.20)] // Maximum 20 items - 20% discount
    public async Task Handle_SaleWithQuantityBasedDiscount_AppliesCorrectDiscount(int quantity, decimal expectedDiscountRate)
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand { Id = saleId };
        var unitPrice = 100m;
    
        var sale = new Sale
        {
            Id = saleId,
            Items = new List<SaleItem>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = quantity,
                    UnitPrice = unitPrice
                }
            }
        };
    
        var expectedDiscount = unitPrice * quantity * expectedDiscountRate;
        var expectedTotal = (unitPrice * quantity) - expectedDiscount;
    
        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
    
        // Configure AutoMapper to map the Sale to GetSaleResult
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GetSaleProfile>();
        });
        var mapper = mapperConfig.CreateMapper();
    
    var expectedResult = new GetSaleResult();
        try {

        expectedResult = mapper.Map<GetSaleResult>(sale);
        }
        catch (Exception e) 
        {
            Console.WriteLine(e);
            
            }

        expectedResult.TotalAmount = expectedTotal;
    
        // Ensure Products is not null
        expectedResult.Products ??= new List<GetSaleItemResult>();
    
        foreach (var p in expectedResult.Products)
        {
            p.Discount = unitPrice * quantity * expectedDiscountRate;
            p.TotalAmount = (unitPrice * quantity) - (unitPrice * quantity * expectedDiscountRate);
        }
    
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
    
        // Then
        result.TotalAmount.Should().Be(expectedTotal);
        var product = result.Products.First();
        product.Discount.Should().Be(expectedDiscount);
        product.TotalAmount.Should().Be(expectedTotal);
        product.Quantity.Should().Be(quantity);
        product.UnitPrice.Should().Be(unitPrice);
        product.ProductId.Should().Be(sale.Items.First().ProductId);
    }

    [Fact]
    public async Task Handle_SaleWithQuantityAbove20_ThrowsValidationException()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand { Id = saleId };
        var sale = new Sale 
        { 
            Id = saleId,
            Items = new List<SaleItem> 
            { 
                new() { Quantity = 21 } 
            }
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);

        // When
        var action = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await action.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("Cannot Sell more than 20 items");
    }}
