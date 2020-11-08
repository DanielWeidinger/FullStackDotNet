using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Devices.Core.Entities;

namespace Devices.Core.Validations
{
    class ToNotBeforeFrom : ValidationAttribute
    {
        public ToNotBeforeFrom() { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Usage usage = (Usage)validationContext.ObjectInstance;
            if (usage.To != null && usage.From > usage.To)
            {
                return new ValidationResult($"To is before From!",
                    new List<string> { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
