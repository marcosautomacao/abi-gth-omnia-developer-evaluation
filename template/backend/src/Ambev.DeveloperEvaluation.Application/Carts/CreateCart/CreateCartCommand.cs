using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public Guid UserId { get; set; }
        public List<CreateSaleItem> Products { get; set; }
    }

    public class CreateSaleItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}