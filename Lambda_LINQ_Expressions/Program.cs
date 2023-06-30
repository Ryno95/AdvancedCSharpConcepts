// See https://aka.ms/new-console-template for more information
using Lambda_LINQ_Expressions;
using Lambda_LINQ_Expressions.Lambda;
using System.Linq.Expressions;

internal class Program
{
	private static void Main(string[] args)
	{
		LambdaEx.Run();
		LinqEx.Run();
		ExpressionTreeEx.Run();
		new AreaExpression().Run();
		FilterBy(nameof(Product.Category),"Electronics");

		Console.WriteLine("Min 555:");
		filterByPrice(555.0m, "min");
		Console.WriteLine("Max 555:");
		filterByPrice(555.0m, "max");
		filterOldSchool(555.0m, "max");

	}

	private static void filterOldSchool(decimal limit, string maxOrMin)
	{
		List<Product> products = GenerateProducts();
		List<Product> filteredProducts = new();

		bool isMax = maxOrMin == "max";
		foreach (Product product in products)
		{
			if (isMax && product.Price < limit)
				filteredProducts.Add(product);
			else if(!isMax && product.Price > limit)
				filteredProducts.Add(product);
		}
		WriteProducts(filteredProducts);
	}

	private static void filterByPrice(decimal limit, string maxOrMin)
	{
		List<Product> products = GenerateProducts();
		List<Product> filteredProducts;
		const string filterField = nameof(Product.Price);
		ParameterExpression parameter = Expression.Parameter(typeof(Product), nameof(Product));
		MemberExpression categoryProperty = Expression.Property(parameter, filterField);
		ConstantExpression limitExpression = Expression.Constant(limit);
		BinaryExpression equalityExpression;
		if (maxOrMin == "max")
			equalityExpression = Expression.LessThanOrEqual(categoryProperty, limitExpression);
		else
			equalityExpression = Expression.GreaterThanOrEqual(categoryProperty, limitExpression);
		Expression<Func<Product, bool>> filterExpression
			= Expression.Lambda<Func<Product, bool>>(equalityExpression, parameter);
		
		filteredProducts = FilterWithExpressions.FilterProducts(products, filterExpression);
		WriteProducts(filteredProducts);
	}

	private static void FilterBy(string filterField, string filterValue)
	{
		// Create a list of sample products
		List<Product> products = GenerateProducts();
		List<Product> filteredProducts;

		// Create a parameter expression representing the input parameter of the lambda expression
		ParameterExpression parameter = Expression.Parameter(typeof(Product), nameof(Product));

		// Create a property access expression for the Category property of the product
		MemberExpression categoryProperty = Expression.Property(parameter, filterField);

		// Create a constant expression representing the value "Electronics"
		ConstantExpression categoryValue = Expression.Constant(filterValue);

		// Create an equality expression comparing the Category property with the constant value
		BinaryExpression equalityExpression = Expression.Equal(categoryProperty, categoryValue);

		// Create the lambda expression by combining the parameter and the equality expression
		Expression<Func<Product, bool>> filterExpression
			= Expression.Lambda<Func<Product, bool>>(equalityExpression, parameter);

		// Filter the products using the filtering condition
		filteredProducts = FilterWithExpressions.FilterProducts(products, filterExpression);

		// Display the filtered products
		WriteProducts(filteredProducts);
	}

	private static void WriteProducts(IEnumerable<Product> products)
	{
		foreach (Product prod in products)
			Console.WriteLine($"Name: {prod.Name}, Price: {prod.Price}, Category: {prod.Category}");

	}

	private static List<Product> GenerateProducts()
	{
		List<Product> products = new List<Product>
		{
			new Product { Name = "iPhone 12", Price = 999.99m, Category = "Electronics" },
			new Product { Name = "Samsung Galaxy S21", Price = 899.99m, Category = "Electronics" },
			new Product { Name = "Sony PlayStation 5", Price = 499.99m, Category = "Gaming" },
			new Product { Name = "Apple AirPods Pro", Price = 249.99m, Category = "Electronics" },
			new Product { Name = "Nike Air Max", Price = 129.99m, Category = "Footwear" },
			new Product { Name = "Amazon Echo Dot", Price = 39.99m, Category = "Electronics" },
			new Product { Name = "Canon EOS Rebel T7i", Price = 749.99m, Category = "Electronics" },
			new Product { Name = "HP Pavilion Laptop", Price = 899.99m, Category = "Electronics" },
			new Product { Name = "Adidas Ultraboost", Price = 179.99m, Category = "Footwear" },
			new Product { Name = "Nintendo Switch", Price = 299.99m, Category = "Gaming" }
		};
		return products;
	}
}

