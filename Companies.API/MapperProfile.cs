using AutoMapper;

using Companies.API.DataTransferObjects;
using Companies.Core.Entities;

namespace Companies.API
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(dto => dto.Address, opt => opt.MapFrom(c => $"{c.Address}{(string.IsNullOrEmpty(c.Country) ? string.Empty : ", ")}{c.Country}"));

            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<Employee, EmployeeDto>();

        }
    }
}