using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;
    private readonly CreateSaleHandler _handler;
    private readonly IMediator _mediator;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventPublisher = Substitute.For<IEventPublisher>();
        _mediator = Substitute.For<IMediator>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _mediator, _eventPublisher);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateCommand();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = Guid.NewGuid().ToString()
        };

        var result = new CreateSaleResult { Id = sale.Id };

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);
        _saleRepository.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // When
        var createResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(sale.Id);
        await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
}
