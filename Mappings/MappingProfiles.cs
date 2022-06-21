
using AutoMapper;
using GlobusAssessment.Application.DTOs;
using GlobusAssessment.Domain.Models;

namespace GLobusAssessment.Api.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.IsCustomerConfirmed, src => src.MapFrom(p => p.PhoneNumberConfirmed))
                .ForMember(dest => dest.State, src => src.MapFrom(p => p.State))
                .ForMember(dest => dest.LGA, src => src.MapFrom(p => p.LGA));

            CreateMap<AddCustomerDto, Customer>();
        }
    }
}
