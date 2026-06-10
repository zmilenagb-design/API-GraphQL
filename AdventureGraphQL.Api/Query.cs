using AdventureGraphQL.Api.Data;
using AdventureGraphQL.Api.Data.Entities;
using HotChocolate.Data;

public class Query
{
	[UsePaging(MaxPageSize = 50)]
	[UseProjection]
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
}