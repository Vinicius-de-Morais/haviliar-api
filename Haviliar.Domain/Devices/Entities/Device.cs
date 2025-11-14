using Haviliar.Domain.Devices.Enums;

namespace Haviliar.Domain.Devices.Entities;

public class Device
{
    public int DeviceId { get; set; }
    public string DeviceName { get; set; }
    public int  NetworkId{ get; set; }
    public DeviceType DeviceType { get; set; }
}
