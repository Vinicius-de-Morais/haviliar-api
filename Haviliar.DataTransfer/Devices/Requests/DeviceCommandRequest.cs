namespace Haviliar.DataTransfer.Devices.Requests;

public record DeviceCommandRequest
{
    public string Serial { get; init; }
    public string Action { get; init; }
}
