using AutoMapper;
using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Dto;

namespace ProjectManagerApi.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<ProjectDto, Project>();
            CreateMap<Project, ProjectGetDto>()
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages.Select(x => x.LanguageId).ToList()))
                .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => src.Technologies.Select(x => x.TechId).ToList()));

            CreateMap<Language, LanguageDto>();
            CreateMap<LanguageDto, Language>();
            CreateMap<AddLanguageDto, Language>();
            CreateMap<Language,AddLanguageDto>();

            CreateMap<Tech,TechDto>();
            CreateMap<TechDto, Tech>();
            CreateMap<AddTechDto, Tech>();
            CreateMap<Tech,AddTechDto>();

        }
    }
}
