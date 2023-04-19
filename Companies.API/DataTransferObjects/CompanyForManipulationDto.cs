using Companies.API.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.API.DataTransferObjects
{
    public abstract record CompanyForManipulationDto 
    {

        [Required(ErrorMessage = "Company name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        [Unique(ErrorMessage = "Name must be unique")]
        public string? Name { get; init; }

        [Required(ErrorMessage = "Company address is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Address is 60 characters")]
        public string? Address { get; set; }
        public string? Country { get; set; }

        public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }

        
    }
}
