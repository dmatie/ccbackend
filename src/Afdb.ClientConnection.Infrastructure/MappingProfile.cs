using AutoMapper;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AccessRequestEntity, AccessRequest>()
            .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects))
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents))
            .ForMember(dest => dest.Function, opt => opt.MapFrom(src => src.Function))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.BusinessProfile, opt => opt.MapFrom(src => src.BusinessProfile))
            .ForMember(dest => dest.FinancingType, opt => opt.MapFrom(src => src.FinancingType))
            .ForMember(dest => dest.ProcessedBy, opt => opt.MapFrom(src => src.ProcessedBy))
            .ReverseMap();

        CreateMap<AccessRequestProjectEntity, AccessRequestProject>().ReverseMap();
        CreateMap<AccessRequestDocumentEntity, AccessRequestDocument>().ReverseMap();
        CreateMap<FunctionEntity, Function>().ReverseMap();
        CreateMap<CountryEntity, Country>().ReverseMap();
        CreateMap<BusinessProfileEntity, BusinessProfile>().ReverseMap();
        CreateMap<FinancingTypeEntity, FinancingType>().ReverseMap();
           CreateMap<UserEntity, User>().ReverseMap();
       }
   }