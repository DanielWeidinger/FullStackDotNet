using Devices.Core.Contracts;
using Devices.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Devices.Core.Entities
{
    public partial class Usage : IEntityObject
    {
        public int Id { get; set; }
        [Timestamp] public byte[] RowVersion { get; set; }
        public DateTime From { get; set; }
        [ToNotBeforeFrom] public DateTime? To { get; set; }
        public int DeviceId { get; set; }
        public int PersonId { get; set; }
        [ForeignKey(nameof(DeviceId))] public Device Device { get; set; }
        [ForeignKey(nameof(PersonId))] public Person Person { get; set; }
    }
}
