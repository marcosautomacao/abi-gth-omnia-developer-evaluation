namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<UpdateSaleItemResult> Products { get; set; }
    }

    public class UpdateSaleItemResult
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}