namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CreateSaleItemResponse> Products { get; set; }
    }

    public class CreateSaleItemResponse
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}