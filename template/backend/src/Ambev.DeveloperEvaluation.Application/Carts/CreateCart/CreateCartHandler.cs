using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IMediator mediator)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            ValidateQuantities(command.Products);

            var sale = _mapper.Map<Sale>(command);
            var updateStockCommands = new List<Task>();
            foreach (var item in sale.Items) 
            {
                var product = await _mediator.Send(new GetProductCommand() { Id = item.ProductId }, cancellationToken);

                if (product == null)
                    throw new ValidationException($"Product not found. Product: {item.ProductId}");

                if (product.Stock < item.Quantity)
                    throw new ValidationException($"Not enough stock for product. Product: {item.ProductId}");

                item.ProductName = product.Description;
                item.UnitPrice = product.Price;  
                
                var updateProductCommand = _mapper.Map<UpdateProductCommand>(product);
                updateProductCommand.Name = product.Description;
                updateProductCommand.Stock -= item.Quantity;
                updateStockCommands.Add(
                    _mediator.Send(updateProductCommand)
                );
            }   

            await Task.WhenAll(
                    updateStockCommands
                );         

            sale.SaleNumber = Guid.NewGuid().ToString();
            sale.SaleDate = DateTime.UtcNow;
            sale.Branch = "OnLineStore";
            var totalSaleAmount = ApplyDiscounts(sale.Items);
            sale.TotalSaleAmount = totalSaleAmount;

            var createdSale = await _saleRepository.AddAsync(sale, cancellationToken);
            var result = _mapper.Map<CreateSaleResult>(createdSale);
            return result;
        }

        private void ValidateQuantities(IEnumerable<CreateSaleItem> items)
        {
            foreach (var item in items)
            {
                if (item.Quantity > 20)
                    throw new ValidationException($"Cannot sell more than 20 items of the same product. Product: {item.ProductId}");
            }
        }

        private decimal ApplyDiscounts(IEnumerable<SaleItem> items)
        {
            decimal totalAmount = 0;
            foreach (var item in items)
            {
                decimal discountRate = 0;

                if (item.Quantity >= 10 && item.Quantity <= 20)
                    discountRate = 0.20m;
                else if (item.Quantity >= 4)
                    discountRate = 0.10m;

                var totalBeforeDiscount = item.UnitPrice * item.Quantity;
                item.Discount = totalBeforeDiscount * discountRate;
                totalAmount = totalBeforeDiscount - item.Discount;
            }
            return totalAmount;
        }
    }
}