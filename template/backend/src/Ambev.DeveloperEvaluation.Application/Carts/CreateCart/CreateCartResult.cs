namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CreateSaleItemResult> Products { get; set; }
    }

    public class CreateSaleItemResult
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}