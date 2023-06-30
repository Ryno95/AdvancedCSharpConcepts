<Query Kind="Program">
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

void Main()
{
   // IServiceContainer
	var container = new DependencyContainer();
	container.AddDependency(typeof(HelloService));
	container.AddDependency<ServiceConsumer>();
	container.AddDependency<MessageService>();

	// IServiceProvider
	var resolver = new DependencyResolver(container);


	var service = resolver.GetService<HelloService>();
	var consumer = resolver.GetService<ServiceConsumer>();
	service.Print();
	consumer.Print();

}

// IServiceProvider
public class DependencyResolver
{
	DependencyContainer _container;
	public DependencyResolver(DependencyContainer container)
	{
		_container = container;
	}

	public T GetService<T>()
	{
		return (T) GetService(typeof(T));
	}

	public object GetService(Type type)
	{
		var dependency = _container.GetDependency(type);
		var constructor = dependency.GetConstructors().Single();
		var paramaters = constructor.GetParameters().ToArray();

		if (paramaters.Length > 0)
		{
			var paramterImplementation = new object[paramaters.Length];
			for (int i = 0; i < paramaters.Length; i++)
			{
				paramterImplementation[i] = GetService(paramaters[i].ParameterType);
			}
			return Activator.CreateInstance(dependency, paramterImplementation)!;
		}

		return Activator.CreateInstance(dependency)!;
	}
}

// IServiceContainer
public class DependencyContainer
{
	List<Type> _dependencies = new List<Type>();

	public void AddDependency(Type type)
	{
		_dependencies.Add(type);
	}

	public void AddDependency<T>()
	{
		_dependencies.Add(typeof(T));
	}

	public Type GetDependency(Type type)
	{
		return _dependencies.First(x => x.Name == type.Name);
	}

}


public class HelloService
{
	MessageService _messageService;
	public HelloService(MessageService messageService)
	{
		_messageService = messageService;
	}

	public void Print()
	{
		Console.WriteLine($"{_messageService.Message()} World");
	}
}

public class ServiceConsumer
{
	HelloService _hello;
	public ServiceConsumer(HelloService hello)
	{
		_hello = hello;
	}

	public void Print()
	{
		_hello.Print();
	}
}

public class MessageService
{
	public string Message()
	{
		return "Yoh!";
	}
}