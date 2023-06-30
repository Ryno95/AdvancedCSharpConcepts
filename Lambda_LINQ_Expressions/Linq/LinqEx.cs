using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lambda_LINQ_Expressions;

//  The following examples are written in both the array method syntaxx and LINQ syntax
public class LinqEx
{
	public LinqEx() { }

	public static void Run()
	{
		FilteringAndProjection();
		Console.WriteLine();

		Aggregation();
		Console.WriteLine();

		Joining();
		Console.WriteLine();

		Sorting();
		Console.WriteLine();

	}

	private static void WriteResult(IEnumerable<int> array)
	{
		foreach (var number in array)
			Console.Write($"{number} ");
		Console.WriteLine();
	}

	private static void FilteringAndProjection()
	{
		int[] numbers = { 1, 2, 3, 4, 5 };

		Console.WriteLine("\nFiltering and Projection:");
		// IEnumrable methods:
		IEnumerable<int> evenNumbers = numbers.Where(num => num % 2 == 0);
		WriteResult(evenNumbers);

		// Using LINQ Query:
		evenNumbers = from num in evenNumbers
					  where num % 2 == 0
					  select num;
		WriteResult(evenNumbers);
		// Result: [2, 4]

		// IEnumrable methods:
		var squaredNumbers = numbers.Select(num => num * num);
		WriteResult(squaredNumbers);
		// Using LINQ Query:
		squaredNumbers = from num in numbers
						 select num * num;
		WriteResult(squaredNumbers);
		// Result: [1, 4, 9, 16, 25]

		var squaredOddNumbers = numbers.Where(num => num % 2 != 0)
									   .Select(num => num * num);
		// IEnumrable methods:
		WriteResult(squaredOddNumbers);

		// Using LINQ Query:
		squaredOddNumbers = from num in numbers
							where num % 2 != 0
							select num * num;
		WriteResult(squaredOddNumbers);
		// Result: [1, 9, 25]
	}

	private static void Aggregation()
	{
		// When using all the values in an IEnumarable, it doesn't makes sense to SELECT them all first
		int[] numbers = { 1, 2, 3, 4, 5 };

		Console.WriteLine("\nAggregation:");

		// and then do the sum.
		int sum = numbers.Sum(); // Result: 15
		Console.WriteLine(sum);

		int max = numbers.Max(); // Result: 5
		Console.WriteLine(max);


		double average = numbers.Average(); // Result: 3
		Console.WriteLine(average);
	}

	private static void Sorting()
	{
		int[] numbers = { 1, 2, 3, 4, 5 };

		var sorted = numbers.OrderByDescending(num => num);
		WriteResult(sorted);

		sorted = from num in numbers
				 orderby num descending
				 select num;
		WriteResult(sorted);

	}

	private static void Joining()
	{
		Console.WriteLine("\nJoining:");
		var employees = new[]
		{
			new Employee { Id = 1, Name = "John" },
			new Employee { Id = 2, Name = "Jane" },
			new Employee { Id = 3, Name = "Bob" }
		};

		var departments = new[]
		{
			new Department { Id = 1, Name = "HR" },
			new Department { Id = 2, Name = "IT" },
			new Department { Id = 3, Name = "Finance" }
		};

		// IEnumrable methods:
		var joinedData = employees.Join(
			departments,
			employee => employee.Id,
			department => department.Id,
			(employee, department) => new { EmployeeName = employee.Name, departmentName = department.Name });

		foreach (var data in joinedData)
			Console.WriteLine($"{data.EmployeeName} - {data.departmentName}");

		// Using LINQ Query:
		joinedData = from employee in employees
					 join department in departments
					 on employee.Id equals department.Id
					 select new { EmployeeName = employee.Name, departmentName = department.Name };
		foreach (var data in joinedData)
			Console.WriteLine($"{data.EmployeeName} - {data.departmentName}");
	}

	public class Employee
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class Department : Employee
	{
	}

}
