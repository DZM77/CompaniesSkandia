using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Core.Entities
{
    public class User : IdentityUser
    {
      //  public Guid Id { get; set; }

        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is a required field.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position is a required field.")]
        [StringLength(maximumLength: 20, MinimumLength = 3, ErrorMessage = "Maximum length for the Position is 20 characters.")]
        public string Position { get; set; } = string.Empty;
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; } = default!;
    }
}
