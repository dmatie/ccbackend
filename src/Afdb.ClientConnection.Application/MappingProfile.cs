using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;

namespace Afdb.ClientConnection.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entities
        CreateMap<OtpCode, OpCodeDto>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => src.ExpiresAt));

        CreateMap<Country, CountryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.NameFr))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<Function, FunctionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<BusinessProfile, BusinessProfileDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<FinancingType, FinancingTypeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.OrganizationName));

        CreateMap<AccessRequestProject, AccessRequestProjectDto>()
            .ForMember(dest => dest.AccessRequestId, opt => opt.MapFrom(src => src.AccessRequestId))
            .ForMember(dest => dest.SapCode, opt => opt.MapFrom(src => src.SapCode));

        CreateMap<AccessRequest, AccessRequestDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.CanBeProcessed, opt => opt.MapFrom(src => src.CanBeProcessed))
            .ForMember(dest => dest.IsProcessed, opt => opt.MapFrom(src => src.IsProcessed))
            .ForMember(dest => dest.HasEntraIdAccount, opt => opt.MapFrom(src => src.HasEntraIdAccount))
            .ForMember(dest => dest.FunctionName, opt => opt.MapFrom(src => src.Function != null ? src.Function.Name : null))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country != null ? src.Country.Name : null))
            .ForMember(dest => dest.BusinessProfileName, 
            opt => opt.MapFrom(src => src.BusinessProfile != null ? src.BusinessProfile.Name : null))
            .ForMember(dest => dest.FinancingTypeName, opt => opt.MapFrom(src => src.FinancingType != null ? src.FinancingType.Name : null))
            .ForMember(dest => dest.SelectedProjectCodes, opt => opt.MapFrom(src => src.Projects.Select(p=>p.SapCode).ToList()));


        CreateMap<ClaimType, ClaimTypeDto>();

        CreateMap<ClaimProcess, ClaimProcessDto>()
            .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User != null ? src.User.FirstName : string.Empty))
            .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.User != null ? src.User.LastName : string.Empty))
            .ForMember(dest => dest.UserFullName, 
                        opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : string.Empty));

        CreateMap<Claim, ClaimDto>()
            .ForMember(dest => dest.ClaimTypeName, opt => opt.MapFrom(src => src.ClaimType != null ? src.ClaimType.Name : string.Empty))
            .ForMember(dest => dest.ClaimTypeNameFr, opt => opt.MapFrom(src => src.ClaimType != null ? src.ClaimType.NameFr : string.Empty))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country != null ? src.Country.Name : string.Empty))
            .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User != null ? src.User.FirstName : string.Empty))
            .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.User != null ? src.User.LastName : string.Empty))
            .ForMember(dest => dest.UserFullName, 
                        opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : string.Empty))
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
            .ForMember(dest => dest.Processes, opt => opt.MapFrom(src => src.Processes));
    }
}