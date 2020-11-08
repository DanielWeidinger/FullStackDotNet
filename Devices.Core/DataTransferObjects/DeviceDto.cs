using Devices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Devices.Core.DataTransferObjects
{
    public class DeviceDto
    {
        public DeviceDto() { }
        public DeviceDto(Device device)
        {
            Id = device.Id;
            SerialNumber = device.SerialNumber;
            Name = device.Name;
            DeviceType = device.DeviceType;
        }

        public int Id { get; set; }
        public string SerialNumber { get; set; }

        public string Name { get; set; }

        public DeviceType DeviceType { get; set; }
    }
}
