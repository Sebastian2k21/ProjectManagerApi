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
        }
    }
}
