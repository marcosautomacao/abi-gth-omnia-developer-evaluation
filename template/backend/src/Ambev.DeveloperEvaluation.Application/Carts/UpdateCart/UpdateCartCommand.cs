using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<UpdateSaleItemCommand> Products { get; set; }
    }

    public class UpdateSaleItemCommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}