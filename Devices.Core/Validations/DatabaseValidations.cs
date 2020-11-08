using Devices.Core.Contracts;
using Devices.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Devices.Core.Validations
{
    class DatabaseValidations
    {
        public static async Task<ValidationResult> FromAndToAreFreeToBookAsync(Usage usage, IUnitOfWork unitOfWork)
        {
            if (await unitOfWork.UsageRepository.AnyBookingOverlappingWithTimespanAsync(usage.DeviceId, usage.From, usage.To))
            {
                return new ValidationResult($"Stating Date overlaps with other booking",
                    new List<string> { nameof(usage.From)  });
            }

            return ValidationResult.Success;
        }
    }
}
