using Devices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Devices.Core.DataTransferObjects
{
    public class PersonDto
    {
        public PersonDto(Person person)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            EmailAddress = person.EmailAddress;
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; } //EmailAddress Annotation/Attrib = Validation; MaxLength = DB Schema

        public string FullName => $"{FirstName} {LastName}";
    }
}
