// See https://aka.ms/new-console-template for more information


using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var container = new ServiceCollection();
container.AddLogging(x => x.AddConsole());

container.AddCap(x =>
{
    //console app does not support dashboard

    x.UseInMemoryStorage();
    x.UseRabbitMQ(o =>{
        o.HostName = "rabbitmq";
    });
});


var sp = container.BuildServiceProvider();

sp.GetService<IBootstrapper>()?.BootstrapAsync();
var capBus = sp.GetService<ICapPublisher>();

for (var i = 0; i < 1000; i++)
{
    capBus?.Publish("place.order.qty.deducted",
        contentObj: new { OrderId = 1234, ProductId = 23255, Qty = 1 });
}

Console.WriteLine("Load Data Done!");