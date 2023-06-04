using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Dto;
using ProjectManagerApi.Exceptions;
using ProjectManagerApi.Services;

namespace ProjectManagerApi.Controllers
{
    [Route("api/tech")]
    [ApiController]
    public class TechController : ControllerBase
    {
        private readonly ITechService techService;
        private readonly IMapper mapper;

        public TechController(ITechService techService, IMapper mapper)
        {
            this.techService = techService;
            this.mapper = mapper;
        }

        [HttpGet("get-all-technologies")]
        public async Task<IActionResult> GetAllTechs()
        {
            return Ok(mapper.Map<List<TechDto>>(await techService.GetAllTechs()));
        }

        [HttpPost("add-new-technology")]
        public async Task<IActionResult> AddNewLanguage(AddTechDto technology)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var tech = await techService.AddNewTech(mapper.Map<Tech>(technology));
                    return Ok(mapper.Map<Tech>(tech));
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
