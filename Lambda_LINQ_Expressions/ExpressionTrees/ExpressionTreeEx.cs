using System.Linq.Expressions;

namespace Lambda_LINQ_Expressions;

public class ExpressionTreeEx
{
	public ExpressionTreeEx() { }

	public static void Run()
	{
		ReadingExpressions();
		CreateExpressions();
	}

	private static void ReadingExpressions()
	{
		string url = "http://example.com/users";

		Console.WriteLine($"With string litterals:    {CreateUrl(url, "name", "age")}"); // mistake prone
		Console.WriteLine($"With expressions strings: {CreateUrl(url, (Student s) => s.Name, s => s.Age)}");
	}

	private static string CreateUrl(string url, params string[] fields)
	{
		var selectedFields = string.Join(',', fields);
		return string.Concat(url, "?fields=", selectedFields);
	}

	private static string CreateUrl(string url, params Expression<Func<Student, object>>[] fieldSelectors)
	{
		var fields = new List<string>();

		foreach (var selector in fieldSelectors)
		{
			var body = selector.Body;
			if (body is MemberExpression me)
			{
				fields.Add(me.Member.Name.ToLower());
			}
			else if (body is UnaryExpression ue)
			{
				fields.Add(((MemberExpression)ue.Operand).Member.Name.ToLower());
			}
		}
		var selectedFields = string.Join(',', fields);
		return string.Concat(url, "?fields=", selectedFields);
	}

	private static void CreateExpressions()
	{
		// value coming from query/header/body
		var selectProperty = "word";
		//var selectProperty = "number";

		var someClass = new SomeClass
		{
			Word = "Hello World",
			Number = 1234
		};

		// hard coded, not scalable
		Console.Write("With if else tree: ");
		if (selectProperty == "word")
		{
			Console.WriteLine(someClass.Word);
		}
		else if (selectProperty == "number")
		{
			Console.WriteLine(someClass.Number);
		}

		//parameter
		Console.Write("Using expressions: ");
		ParameterExpression parameter = Expression.Parameter(typeof(SomeClass));
		MemberExpression	accessor = Expression.PropertyOrField(parameter, selectProperty);
		LambdaExpression	lambda = Expression.Lambda(accessor, false, parameter);
		Console.WriteLine(lambda.Compile().DynamicInvoke(someClass));
	}
}

public class SomeClass
{
	public string? Word { get; set; }
	public int Number { get; set; }
}
