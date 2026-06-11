using AdventureGraphQL.Api.Data;
using AdventureGraphQL.Api.Data.Entities;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;

public class Query
{
    [UsePaging(MaxPageSize = 50)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Product> GetProducts(AdventureWorksContext context)
        => context.Products;

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Customer> GetCustomers(AdventureWorksContext context)
        => context.Customers;

    [UseProjection]
    public IQueryable<Product> GetProductById(int id, AdventureWorksContext context)
        => context.Products.Where(p => p.ProductID == id);

    public IQueryable<Customer> GetTopCustomers(
        int year,
        AdventureWorksContext context)
        => context.Customers
            .Include(c => c.SalesOrderHeaders)
            .Where(c => c.SalesOrderHeaders
                .Any(o => o.OrderDate.Year == year))
            .OrderByDescending(c => c.SalesOrderHeaders
                .Where(o => o.OrderDate.Year == year)
                .Sum(o => o.TotalDue));
}