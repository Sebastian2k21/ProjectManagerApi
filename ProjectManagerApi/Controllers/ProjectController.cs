using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Dto;
using ProjectManagerApi.Exceptions;
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
            return Ok(await projectService.GetAllProjects());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDto projectDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var project = await projectService.CreateProject(int.Parse(userId) ,mapper.Map<Project>(projectDto), projectDto.TechnologiesList, projectDto.LanguagesList);
                    return Ok(project);
                }
                catch (InvalidItemIdException e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
