using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using RazorLight;
using CNet.CodeGen.Api.Util;

namespace CNet.CodeGen.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public   IEnumerable<WeatherForecast> Get()
        {
            // Razor 模板文件路径
            //GenByRazor.CompileModel("Pub_Role", "");
            //GenByRazor.CompileBLL("Pub_Role","");
            // GenByRazor.CompileAdminController("Pub_Role", "");
            GenByRazor.CompileAdminUI("Pub_Role");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
          .ToArray();
        }
    }
}
