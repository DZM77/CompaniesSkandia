using System.ComponentModel.DataAnnotations;

namespace Companies.API.DataTransferObjects
{
    public record EmployeeForCreationDto : EmployeeForManipulationDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; init; } = default!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; } = default!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; init; } = default!;
        public string? PhoneNumber { get; init; }

    }
}