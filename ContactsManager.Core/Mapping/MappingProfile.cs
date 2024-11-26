using AutoMapper;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Routing.Constraints;

namespace ContactsManager.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Country
            CreateMap<CountryAddRequestDto, Country>();
            CreateMap<Country, CountryResponseDto>();
           
            #endregion

            #region Person
            CreateMap<PersonAddRequestDto, Person>();
            CreateMap<Person, PersonResponseDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => (DateTime.Now.Year - Convert.ToDateTime(src.Dob).Year)))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country == null ? null : src.Country.CountryName));


            CreateMap<PersonUpdateRequestDto, Person>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<RegisterDTO,ApplicationUser>()
                .ForMember(dest=>dest.PhoneNumber, opt=>opt.MapFrom(src=>src.Phone))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            #endregion
        }
    }
}
