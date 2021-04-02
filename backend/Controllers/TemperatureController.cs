using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using backend.Models;

namespace backend.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class TemperatureController : Controller
    {
        private readonly ILogger<TemperatureController> _logger;

        public TemperatureController(ILogger<TemperatureController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetData()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveData(TemperatureMeasurement temperatureMeasurement)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (ShouldFail())
            {
                _logger.LogInformation("Processing request intentionally failed");
                return StatusCode(500);
            }

            await StoreData(temperatureMeasurement);

            return Ok();
        }

        [HttpPost("missing")]
        public async Task<IActionResult> ReceiveMissingData(List<TemperatureMeasurement> temperatureMeasurements)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (ShouldFail())
            {
                _logger.LogInformation("Processing request intentionally failed");
                return StatusCode(500);
            }

            foreach (var measurement in temperatureMeasurements)
                await StoreData(measurement);

            return Ok();

        }

        private bool ShouldFail()
        {
            return new Random().Next(1, 11) > 6; // 40%
        }

        private async Task StoreData(TemperatureMeasurement measurement)
        {
            await System.IO.File.AppendAllTextAsync("output.txt", measurement.ToString() + Environment.NewLine);
        }
    }
}
