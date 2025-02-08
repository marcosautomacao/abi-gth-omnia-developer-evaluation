namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequest
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<UpdateSaleItemRequest> Products { get; set; }
    }

    public class UpdateSaleItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}