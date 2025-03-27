using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly UpdateSaleHandler _handler;
    private readonly IEventPublisher _eventPublisher;

    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _eventPublisher);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = new UpdateSaleCommand { Id = saleId, UserId = Guid.NewGuid(), Date = DateTime.UtcNow };
        var sale = new Sale { Id = saleId };
        var result = new UpdateSaleResult { Id = saleId };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<UpdateSaleResult>(sale).Returns(result);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // When
        var updateResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(saleId);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
}
