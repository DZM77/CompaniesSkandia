using Microsoft.AspNetCore.Identity;

namespace Companies.API.Exceptions
{
    public class BadRequestException : Exception
    {
        public Dictionary<string, string> Errors { get; } = new();
        public BadRequestException(string message, IEnumerable<IdentityError> errors) : base(message)
        {
            foreach (var error in errors)
            {
                Errors.Add(error.Code, error.Description);
            }
        }

    }
}
