using Devices.Core.DataTransferObjects;
using Devices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devices.Web.DataTransferObjects
{
    public class UsageDto
    {
        public UsageDto() { }

        public UsageDto(Usage usage)
        {
            Id = usage.Id;
            From = usage.From;
            To = usage.To;
            Person = new PersonDto(usage.Person);
            Device = new DeviceDto(usage.Device);
        }

        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }

        public PersonDto Person { get; set; }
        public DeviceDto Device { get; set; }
    }
}
