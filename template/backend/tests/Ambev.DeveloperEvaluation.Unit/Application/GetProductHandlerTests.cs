using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductHandler _handler;

    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductHandler(_productRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ExistingProduct_ReturnsProduct()
    {
        // Given
        var productId = Guid.NewGuid();
        var command = new GetProductCommand { Id = productId };
        var product = new Product { Id = productId };
        var result = new GetProductResult { Id = productId };

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(result);

        // When
        var getResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getResult.Should().NotBeNull();
        getResult.Id.Should().Be(productId);
    }
}
