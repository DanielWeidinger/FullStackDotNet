using Devices.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Devices.Core.Entities
{
    public class Device : IEntityObject
    {
        public int Id { get; set; }
        
        [Timestamp] public byte[] RowVersion { get; set; }

        [MaxLength(100)] public string SerialNumber { get; set; }
        
        [Required, MaxLength(40)] public string Name { get; set; }

        public DeviceType DeviceType { get; set; }

        public ICollection<Usage> Usages { get; set; } = new List<Usage>();
    }
}
