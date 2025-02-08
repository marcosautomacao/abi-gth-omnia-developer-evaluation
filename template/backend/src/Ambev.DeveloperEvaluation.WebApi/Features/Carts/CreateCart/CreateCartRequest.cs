namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public Guid UserId { get; set; }
        // public DateTime Date { get; set; }
        public List<CreateSaleItemRequest> Products { get; set; }
    }

    public class CreateSaleItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}