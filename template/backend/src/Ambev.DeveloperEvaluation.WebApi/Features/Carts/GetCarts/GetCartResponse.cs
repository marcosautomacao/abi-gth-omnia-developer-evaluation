namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSaleResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<GetSaleItemResponse> Products { get; set; }
    }

    public class GetSaleItemResponse
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}