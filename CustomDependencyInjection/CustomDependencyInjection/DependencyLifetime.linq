<Query Kind="Program" />

void Main()
{
// IServiceContainer
		var container = new DependencyContainer();
		container.AddScoped<ServiceConsumer>();
		container.AddScoped<HelloService>();
		container.AddSingleton<MessageService>();

		// IServiceProvider
		var resolver = new DependencyResolver(container);



		var service1 = resolver.GetService<HelloService>();
		var service2 = resolver.GetService<HelloService>();
		var service3 = resolver.GetService<HelloService>();

		var consumer = resolver.GetService<ServiceConsumer>();

		// print random number three times to show
		// every instance is a unique instance
		service1.Print();
		service2.Print();
		service3.Print();



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
			return (T)GetService(typeof(T));
		}

		public object GetService(Type type)
		{
			var dependency = _container.GetDependency(type);
			var constructor = dependency.Type.GetConstructors().Single();
			var paramaters = constructor.GetParameters().ToArray();

			if (paramaters.Length > 0)
			{
				var paramterImplementation = new object[paramaters.Length];
				for (int i = 0; i < paramaters.Length; i++)
				{
					paramterImplementation[i] = GetService(paramaters[i].ParameterType);
				}
				return CreateImplementation(dependency, factory => Activator.CreateInstance(dependency.Type, paramterImplementation)!);
			}
			return CreateImplementation(dependency, factory => Activator.CreateInstance(dependency.Type)!);
		}

		public object CreateImplementation(Dependency dependency, Func<Type, object> factory)
		{

			if (dependency.IsImplemented)
				return dependency.Implementation;

			var implementation = factory(dependency.Type)!;
			if (dependency.Lifetime == DependencyLifetime.Singleton)
			{
				dependency.AddImplementation(implementation);
			};

			return implementation;
		}
	}

	// IServiceContainer
	public class DependencyContainer
	{
		List<Dependency> _dependencies;

		public DependencyContainer()
		{
			_dependencies = new List<Dependency>();
		}
		public void AddSingleton<T>()
		{
			_dependencies.Add(new Dependency(typeof(T), DependencyLifetime.Singleton));
		}

		public void AddScoped<T>()
		{
			_dependencies.Add(new Dependency(typeof(T), DependencyLifetime.Scoped));
		}

		public Dependency GetDependency(Type type)
		{
			return _dependencies.First(x => x.Type.Name == type.Name);
		}

	}

	public class Dependency
	{
		public Dependency(Type type, DependencyLifetime lifetime)
		{
			Type = type;
			Lifetime = lifetime;
		}

		public Type Type { get; set; }
		public DependencyLifetime Lifetime { get; set; }
		public object Implementation { get; set; }
		public bool IsImplemented { get; set; }

		public void AddImplementation(object implemantation)
		{
			IsImplemented = true;
			Implementation = implemantation;
		}
	}

	public enum DependencyLifetime
	{
		Singleton = 0,
		Scoped = 1
	}
	public class HelloService
	{
		MessageService _messageService;
		int _random;
		public HelloService(MessageService messageService)
		{
			_messageService = messageService;
			_random = new Random().Next();
		}

		public void Print()
		{
			Console.WriteLine($"{_messageService.Message()} World {_random}");
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
		int _random;

		public MessageService()
		{
			_random = new Random().Next();
		}
		public string Message()
		{
			return $"{_random} Yoh!";
		}
	}


}
