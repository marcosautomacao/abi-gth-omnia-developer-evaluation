namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<UpdateSaleItemResponse> Products { get; set; }
    }

    public class UpdateSaleItemResponse
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}