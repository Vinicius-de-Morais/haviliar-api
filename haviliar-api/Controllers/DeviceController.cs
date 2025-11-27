namespace haviliar_api.Controllers;

using System.Text.Json;
using Haviliar.DataTransfer.Devices.Requests;
using haviliar_api.MQTT;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly MqttService _mqttService;

    public DevicesController(MqttService mqttService)
    {
        _mqttService = mqttService;
    }

    [HttpPost("command")]
    public async Task<IActionResult> SendCommand([FromBody] DeviceCommandRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Serial) ||
            string.IsNullOrWhiteSpace(request.Action))
        {
            return BadRequest("Serial e action são obrigatórios.");
        }

        var topic = "esp32/open";


        await _mqttService.PublishAsync(topic, "open");

        return Ok(new
        {
            success = true,
            topic
        });
    }
}

