using Companies.API.Data;
using Companies.API.DataTransferObjects;
using System.ComponentModel.DataAnnotations;

namespace Companies.API.Validations
{
    public class Unique : ValidationAttribute
    {
      
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (string)value!;

            var dto = validationContext.ObjectInstance as CompanyForManipulationDto;

            if (dto != null)
            {

                var context = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

                ArgumentNullException.ThrowIfNull(nameof(context));

                if (!context!.Companies.Any(c => c.Name!.Equals(currentValue)))
                    return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
