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
    [Route("api/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IProjectService projectService;

        public ProjectController(IMapper mapper, IProjectService projectService)
        {
            this.mapper = mapper;
            this.projectService = projectService;
        }

        [HttpGet("get-all-projects")]
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
        public async Task<IActionResult> AddUserToProject(TeamUserDto addUserDto)
        {
            if(ModelState.IsValid)
            {
                var leaderId = User.GetUserId();
                await projectService.AddUserToProject(addUserDto.ProjectId, leaderId, addUserDto.UserId, addUserDto.RoleId);
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("change-role")]
        public async Task<IActionResult> ChangeRole(TeamUserDto addUserDto)
        {
            if (ModelState.IsValid)
            {
                var leaderId = User.GetUserId();
                await projectService.ChangeUserRole(addUserDto.ProjectId, leaderId, addUserDto.UserId, addUserDto.RoleId);
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost("add-status")]
        public async Task<IActionResult> AddStatus(ProjectStatusDto projectStatusDto)
        {
            if (ModelState.IsValid)
            {
                var leaderId = User.GetUserId();
                await projectService.SetProjectStatus(projectStatusDto.ProjectId, leaderId, projectStatusDto.StatusId);
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpGet("projects-with-private-recruitment")]
        public async Task<IActionResult> GetAllProjectWithPrivateRecruitment()
        {
            return Ok(mapper.Map<List<ProjectGetDto>>(await projectService.GetAllProjectWithPrivateRecruitment()));
        }
    }
}
