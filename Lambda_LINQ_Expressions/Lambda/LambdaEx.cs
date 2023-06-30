namespace Lambda_LINQ_Expressions.Lambda;
public class LambdaEx
{
	public LambdaEx() { }

	public static void Run()
	{
		// Example 1: Sorting a list of strings using a lambda expression
		List<string> fruits = new List<string> { "apple", "banana", "cherry", "date" };
		fruits.Sort((x, y) => x.Length.CompareTo(y.Length));
		Console.WriteLine(string.Join(", ", fruits));
		// Output: date, apple, banana, cherry

		// Example 2: Filtering a list of numbers using a lambda expression
		List<int> numbers = new() { 1, 2, 3, 4, 5 };
		List<int> evenNumbers = numbers.FindAll(x => x % 2 == 0);
		Console.WriteLine(string.Join(", ", evenNumbers));
		// Output: 2, 4

		// Example 3: Using lambda expressions with LINQ
		var students = new List<Student>
	{
		new Student { Name = "Alice", Age = 20 },
		new Student { Name = "Bob", Age = 18 },
		new Student { Name = "Charlie", Age = 22 },
		new Student { Name = "David", Age = 19 }
	};

		var result = students.Where(s => s.Age > 18).OrderBy(s => s.Name);
		foreach (var student in result)
		{
			Console.WriteLine($"{student.Name} ({student.Age})");
		}
	}
}
