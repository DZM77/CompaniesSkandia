using AutoMapper;

using Companies.API.DataTransferObjects;
using Companies.API.Entities;

namespace Companies.API
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            //CompanyMappings
            CreateMap<Company, CompanyDto>()
                .ForMember(dto => dto.Address, opt => opt.MapFrom(c => $"{c.Address}{(string.IsNullOrEmpty(c.Country) ? string.Empty : ", ")}{c.Country}"));

            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<CompanyForUpdateDto, Company>();


            //EmployeeMappings
            CreateMap<EmployeeForCreationDto, User>();
            CreateMap<EmployeeForUpdateDto, User>().ReverseMap();
            CreateMap<User, EmployeeDto>();

        }
    }
}