using Haviliar.DataTransfer.Devices.Responses;
using haviliar_api.WebsocketHub;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Protocol;
using System.Text;
using System.Text.Json;

namespace haviliar_api.MQTT;

public class MqttService : IHostedService
{
    private IMqttClient _mqttClient;
    private readonly string _broker = "10.156.205.199";
    private readonly int _port = 1883;
    private readonly string _clientId = $"api-haviliar";
    private readonly IHubContext<DevicesHub> _hubContext;

    public MqttService(
        IHubContext<DevicesHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new MqttClientFactory();
        _mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(_broker, _port)
            .WithClientId(_clientId)
            .Build();

        _mqttClient.ApplicationMessageReceivedAsync += HandleMessageReceived;

        _mqttClient.ConnectedAsync += async e =>
        {
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic("devices/status")
                .Build());
        };

        _mqttClient.DisconnectedAsync += async e =>
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            try
            {
                await _mqttClient.ConnectAsync(options, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao reconectar MQTT: {ex.Message}");
            }
        };

        await _mqttClient.ConnectAsync(options, cancellationToken);
    }

    private async Task HandleMessageReceived(MqttApplicationMessageReceivedEventArgs e)
    {
        var topic = e.ApplicationMessage.Topic;
        var payloadBytes = e.ApplicationMessage.Payload;
        var payload = Encoding.UTF8.GetString(payloadBytes);

        Console.WriteLine(payload);

        if (topic == "devices/status")
        {
            try
            {
                var device = ParseWeirdDevicePayload(payload);

                if (device is not null)
                {
                    await _hubContext.Clients.All
                        .SendAsync("deviceStatusUpdated", device);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao reconectar MQTT: {ex.Message}");
            }
        }
    }

    public async Task PublishAsync(string topic, string payload)
    {
        if (_mqttClient == null || !_mqttClient.IsConnected)
        {
            Console.WriteLine("MQTT client is not connected. Cannot publish message.");
            return;
        }

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        await _mqttClient.PublishAsync(message);
        Console.WriteLine("Published MQTT message. Topic: {Topic} Payload: {Payload}",
            topic, payload);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_mqttClient != null)
            await _mqttClient.DisconnectAsync();
    }


    private DeviceStatusMessage ParseWeirdDevicePayload(string payload)
    {
        if (payload.StartsWith("\"") && payload.EndsWith("\""))
        {
            payload = payload.Substring(1, payload.Length - 2);
        }

        payload = payload.Trim();

        if (payload.StartsWith("{") && payload.EndsWith("}"))
        {
            payload = payload.Substring(1, payload.Length - 2);
        }

        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var parts = payload.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var kv = part.Split(':', 2);
            if (kv.Length != 2) continue;

            var key = kv[0].Trim();
            var value = kv[1].Trim();

            dict[key] = value;
        }

        var device = new DeviceStatusMessage
        {
            Serial = dict.GetValueOrDefault("serial") ?? string.Empty,
            OperationCenterId = dict.GetValueOrDefault("operationCenterId") ?? string.Empty,
            NetworkId = dict.GetValueOrDefault("networkId") ?? string.Empty,
            Firmware = dict.GetValueOrDefault("firmware") ?? string.Empty,
            Status = bool.TryParse(dict.GetValueOrDefault("status"), out var status) && status
        };

        return device;
    }
}
