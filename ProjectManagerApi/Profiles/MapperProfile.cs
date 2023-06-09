﻿using AutoMapper;
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
            CreateMap<ProjectUpdateDto, Project>();
            CreateMap<Project, ProjectGetDto>()
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages.Select(x => x.LanguageId).ToList()))
                .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => src.Technologies.Select(x => x.TechId).ToList()));

            CreateMap<User, UserEditDto>();

            CreateMap<Language, LanguageDto>();
            CreateMap<LanguageDto, Language>();
            CreateMap<AddLanguageDto, Language>();
            CreateMap<Language,AddLanguageDto>();

            CreateMap<Tech,TechDto>();
            CreateMap<TechDto, Tech>();
            CreateMap<AddTechDto, Tech>();
            CreateMap<Tech,AddTechDto>();

            CreateMap<User, UserGetDto>();

            CreateMap<TeamUser, ProjectMemberDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role!.Name))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User!.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User!.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User!.Email));

        }
    }
}
