using Devices.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Devices.Core.Entities
{
    public class Person : IEntityObject
    {
        public int Id { get; set; }

        [Timestamp] public byte[] RowVersion { get; set; }

        [Required, MaxLength(40)] public string FirstName { get; set; }

        [Required, MaxLength(100)] public string LastName { get; set; }

        [EmailAddress, MaxLength(60)] public string EmailAddress { get; set; } //EmailAddress Annotation/Attrib = Validation; MaxLength = DB Schema

        public string FullName => $"{FirstName} {LastName}";

        public ICollection<Usage> Usages { get; set; } = new List<Usage>();
    }
}
