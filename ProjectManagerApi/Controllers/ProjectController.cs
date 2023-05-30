using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Dto;
using ProjectManagerApi.Exceptions;
using ProjectManagerApi.Extensions;
using ProjectManagerApi.Services;
using System.Security.Claims;

namespace ProjectManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IProjectService projectService;

        public ProjectController(IMapper mapper, IProjectService projectService)
        {
            this.mapper = mapper;
            this.projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            return Ok(mapper.Map<List<ProjectGetDto>>(await projectService.GetAllProjects()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDto projectDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int userId = User.GetUserId();
                    var project = await projectService.CreateProject(userId ,mapper.Map<Project>(projectDto), projectDto.TechnologiesList, projectDto.LanguagesList);
                    return Ok(mapper.Map<ProjectGetDto>(project));
                }
                catch (InvalidItemIdException e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUserToProject(AddUserDto addUserDto)
        {
            if(ModelState.IsValid)
            {
                var leaderId = User.GetUserId();
                await projectService.AddUserToProject(addUserDto.ProjectId, leaderId, addUserDto.UserId, addUserDto.RoleId);
                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
