using AdventureGraphQL.Api.Data;
using AdventureGraphQL.Api.Data.Entities;
using HotChocolate.Subscriptions;

public record AddProductInput(
	string Name,
	string ProductNumber,
	decimal ListPrice,
	int? ProductSubcategoryId);

public record AddProductPayload(
	int ProductId,
	string Name,
	decimal ListPrice);

public class Mutation
{
	public async Task<AddProductPayload> AddProductAsync(
		AddProductInput input,
		AdventureWorksContext context,
		[Service] ITopicEventSender sender,
		CancellationToken ct)
	{
		if (input.ListPrice < 0)
			throw new GraphQLException("El precio no puede ser negativo.");

		var product = new Product
		{
			Name = input.Name,
			ProductNumber = input.ProductNumber,
			ListPrice = input.ListPrice,
			ProductSubcategoryID = input.ProductSubcategoryId,
			SellStartDate = DateTime.UtcNow,
			ModifiedDate = DateTime.UtcNow,
			rowguid = Guid.NewGuid(),
			// Campos requeridos por constraints de AdventureWorks
			MakeFlag = false,
			FinishedGoodsFlag = false,
			SafetyStockLevel = 1,
			ReorderPoint = 1,
			StandardCost = 0,
			DaysToManufacture = 0
		};

		context.Products.Add(product);
		await context.SaveChangesAsync(ct);

		var payload = new AddProductPayload(
			product.ProductID, product.Name, product.ListPrice);

		await sender.SendAsync(nameof(Subscription.OnProductAdded), payload, ct);

		return payload;
	}
}
