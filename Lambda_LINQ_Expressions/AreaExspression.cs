
using System.Linq.Expressions;

namespace Lambda_LINQ_Expressions;

public class Square
{
	public double Width {get; set;}
	public double Height { get; set; }

}
public class AreaExpression
{
	public void Run()
	{
		Func<double, double, double> calculateArea = GenerateAreaExpression().Compile();
		const double width = 333;
		const double height = 2;
        Console.WriteLine(calculateArea(width, height));
    }
	public Expression<Func<double, double, double>> GenerateAreaExpression()
	{
        Console.WriteLine($"Should be 'height': {nameof(Square.Height).ToLower()}");
		Console.WriteLine($"Should be 'width': {nameof(Square.Width).ToLower()}");

        // define parameters for expression
		ParameterExpression width = Expression.Parameter(typeof(double), nameof(Square.Width).ToLower());
		ParameterExpression heigth = Expression.Parameter(typeof(double), nameof(Square.Height).ToLower());
		BinaryExpression exprBody = Expression.Multiply(heigth, width);
		Expression<Func<double, double, double>> lambdaExpr 
			= Expression.Lambda<Func<double, double, double>>(exprBody, width, heigth);
		return lambdaExpr;
	}
}
