using AdventureGraphQL.Api.Data;
using AdventureGraphQL.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdventureGraphQL.Api;

[ExtendObjectType(typeof(Product))]
public class ProductExtensions
{
    [IsProjected(true)]
    public int? GetProductSubcategoryID([Parent] Product product)
        => product.ProductSubcategoryID;

    public async Task<string?> GetCategory(
        [Parent] Product product,
        [Service] IDbContextFactory<AdventureWorksContext> factory)
    {
        if (product.ProductSubcategoryID == null)
            return null;

        await using var context = factory.CreateDbContext();

        var subcategory = await context.ProductSubcategories
            .Include(s => s.ProductCategory)
            .FirstOrDefaultAsync(s => s.ProductSubcategoryID == product.ProductSubcategoryID);

        return subcategory?.ProductCategory?.Name;
    }
}