using System.Text.Json;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace WebCAP.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ICapPublisher _capBus;
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ICapPublisher capBus)
    {
        _logger = logger;
        _capBus = capBus;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {

        for (int i = 0; i < 100; i++)
        {
            _capBus.Publish("place.order.qty.deducted",
                contentObj: new { OrderId = 1234, ProductId = 23255, Qty = 1 });
            _logger.LogInformation("enviando");
        }

        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [CapSubscribe("place.order.qty.deducted")]
    public object DeductProductQty(JsonElement param)
    {
        var orderId = param.GetProperty("OrderId").GetInt32();
        var productId = param.GetProperty("ProductId").GetInt32();
        var qty = param.GetProperty("Qty").GetInt32();

        Thread.Sleep(1000);
        //business logic 

        return new { OrderId = orderId, IsSuccess = true };
    }
}