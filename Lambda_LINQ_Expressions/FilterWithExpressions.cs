using System.Linq.Expressions;

namespace Lambda_LINQ_Expressions
{

	public class Product
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Category { get; set; }
	}

	public static class FilterWithExpressions
	{
		public static List<Product> FilterProducts(
				List<Product> products, Expression<Func<Product, bool>> filterCondition
			)
		{
			List<Product> result = new();
			// Compile the expression tree into a delegate
			Func<Product, bool> filterFunc = filterCondition.Compile();

			// Filter the products using the compiled delegate
			foreach (Product product in products)
			{
				if (filterFunc(product))
					result.Add(product);
			}
			// Return the filtered products
			return result;
		}
	}

}

