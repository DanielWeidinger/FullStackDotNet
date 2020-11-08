using Devices.Core.Contracts;
using Devices.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Devices.Core.Entities
{
    public partial class Usage : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var unitOfWork = validationContext.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            if (unitOfWork == null)
            {
                throw new AccessViolationException("UnitOfWork is not injected!");
            }
            var validationResults = DatabaseValidations.FromAndToAreFreeToBookAsync(this, unitOfWork).Result;
            if (validationResults != ValidationResult.Success)
            {
                yield return validationResults;
            }
        }
    }
}
