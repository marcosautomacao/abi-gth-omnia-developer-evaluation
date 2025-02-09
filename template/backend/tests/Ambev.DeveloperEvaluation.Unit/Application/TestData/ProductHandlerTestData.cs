using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class ProductHandlerTestData
{
    private static readonly Faker<CreateProductCommand> createProductFaker = new Faker<CreateProductCommand>()
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
        .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(1, 1000)))
        .RuleFor(p => p.Count, f => f.Random.Number(1, 1000));

    private static readonly Faker<UpdateProductCommand> updateProductFaker = new Faker<UpdateProductCommand>()
        .RuleFor(p => p.Id, f => Guid.NewGuid())
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
        .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(1, 1000)))
        .RuleFor(p => p.Count, f => f.Random.Number(1, 1000));

    private static readonly Faker<GetProductCommand> getProductFaker = new Faker<GetProductCommand>()
        .RuleFor(p => p.Id, f => Guid.NewGuid());

    private static readonly Faker<DeleteProductCommand> deleteProductFaker = new Faker<DeleteProductCommand>()
        .RuleFor(p => p.Id, f => Guid.NewGuid());

    public static CreateProductCommand GenerateValidCreateCommand()
    {
        return createProductFaker.Generate();
    }

    public static UpdateProductCommand GenerateValidUpdateCommand()
    {
        return updateProductFaker.Generate();
    }

    public static GetProductCommand GenerateValidGetCommand()
    {
        return getProductFaker.Generate();
    }

    public static DeleteProductCommand GenerateValidDeleteCommand()
    {
        return deleteProductFaker.Generate();
    }
}