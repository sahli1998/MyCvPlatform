using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonCv.IRepositories;
using MonCv.Repositories;

namespace MonCv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IRepoProject _RepoProject;
        public ProjectController(IRepoProject repoProject)
        {
            _RepoProject = repoProject;
        }

        [HttpGet]
        public async  Task<IActionResult> GetAllProject()
        {
            return Ok(await _RepoProject.GetAllProjects());

        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            return Ok(await _RepoProject.GetProjectById(id));

        }

        [HttpPost]
        public async Task<IActionResult> AddNewProject(ProjectAddDTO dto)
        {
            return  Ok( _RepoProject.AddNewProject(dto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(int a, [FromBody] ProjectAddDTO dto)
        {
            return Ok(_RepoProject.UpdateProject(a, dto));
        }

        [HttpDelete]
        public async Task<IActionResult> Removed(int id)
        {
            return Ok(_RepoProject.RemoveProject(id));
        }

        [HttpPost("SpecialAdd")]
        public async Task<IActionResult> AddProject([FromForm] ProjectFormData dto)
        {
            return Ok(_RepoProject.AddProject(dto));
        }
    }
}
