<Query Kind="Program" />

void Main()
{
	// for DI we want to get rid of this new keyword for intanciating a service.
	// So we create a Dependency Injection container where we can register all these services

	//var service = new HelloService();
	//var consumer = new ServiceConsumer(service);


	// This can be seen as a simple form of manual DI as we still need to pass params
	var serType = typeof(HelloService);
	serType.Dump();
	var service = Activator.CreateInstance(typeof(HelloService)) as HelloService;
	var consumer = Activator.CreateInstance(typeof(ServiceConsumer), service) as ServiceConsumer;

	service!.Print();
	consumer!.Print();
}

public class HelloService
{
	public void Print()
	{
		"Hello World".Dump();
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

