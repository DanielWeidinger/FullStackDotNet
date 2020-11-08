using Devices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Devices.Core
{
    class ImportController
    {

        const string Filename = "Usages.csv";

        public static async Task<IEnumerable<Usage>> ReadFromCsvAsync()
        {
            string[][] matrix = await MyFile.ReadStringMatrixFromCsvAsync(Filename, true);
            var persons = matrix
                .GroupBy(strings => strings[5])
                .Select(grp => new Person
                {
                    EmailAddress = grp.Key,
                    FirstName = grp.First()[3],
                    LastName = grp.First()[4]
                })
                .ToArray();

            var devices = matrix
                .GroupBy(strings => strings[0])
                .Select(grp => new Device
                {
                    SerialNumber = grp.Key,
                    DeviceType = (DeviceType)Enum.Parse(typeof(DeviceType), grp.First()[2]),
                    Name = grp.First()[1]
                })
                .ToArray();

            var usages = matrix
                .Select(line => new Usage
                {
                    From = Convert.ToDateTime(line[6]),
                    To = line[7] != "" ? Convert.ToDateTime(line[7]) : (DateTime?)null,
                    Device = devices.First(device => device.SerialNumber == line[0]),
                    Person = persons.First(person => person.EmailAddress == line[5])
                })
                .ToArray();

            return usages;
        }

    }
}
