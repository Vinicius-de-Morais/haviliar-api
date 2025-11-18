namespace Haviliar.DataTransfer.Devices.Responses;

public record DeviceStatusMessage
{
    public string Serial { get; set; } = default!;
    public string OperationCenterId { get; set; } = default!;
    public string NetworkId { get; set; } = default!;
    public string Firmware { get; set; } = default!;
    public bool Status { get; set; }
}
