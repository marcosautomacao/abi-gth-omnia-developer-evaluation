namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<GetSaleItemResult> Products { get; set; }
    }

    public class GetSaleItemResult
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}