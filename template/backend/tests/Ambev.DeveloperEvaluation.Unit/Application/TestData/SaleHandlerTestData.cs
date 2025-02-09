using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class SaleHandlerTestData
{
    private static readonly Faker<CreateSaleCommand> createSaleFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.UserId, f => Guid.NewGuid())
        .RuleFor(s => s.Products, f => GenerateItems(f.Random.Number(1, 5)));

    private static readonly Faker<CreateSaleItem> saleItemFaker = new Faker<CreateSaleItem>()
        .RuleFor(i => i.ProductId, f => Guid.NewGuid())
        .RuleFor(i => i.Quantity, f => f.Random.Number(1, 10));

    private static List<CreateSaleItem> GenerateItems(int count)
    {
        return saleItemFaker.Generate(count);
    }

    public static CreateSaleCommand GenerateValidCreateCommand()
    {
        return createSaleFaker.Generate();
    }
}
