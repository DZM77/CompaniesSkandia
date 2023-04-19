using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.API.DataTransferObjects
{
    public record CompanyDto()
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Address { get; init; }
        //  public string? Country { get; init; }

        public IEnumerable<EmployeeDto>? Employees { get; init; }

    }
}
