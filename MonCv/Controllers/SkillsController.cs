using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonCv.Data;
using MonCv.Dtos;
using MonCv.IRepositories;
using MonCv.Model;

namespace MonCv.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class SkillsController : Controller
    {
        private readonly IRespoSkills _RepoSkills;
        public SkillsController(IRespoSkills RepoSkillsb)
        {
           _RepoSkills= RepoSkillsb;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_RepoSkills.GetAllSkills());
        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(_RepoSkills.GetSkillsById(id));
        }


        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string Name)
        {
            return Ok(_RepoSkills.GetSkillsByName(Name));
        }

        [HttpPost("AddSkill")]
        public async Task<IActionResult> AddSkill(DtoSkill skill)
        {
            Skills skill1 = new Skills();
            skill1.Name = skill.Name;
            skill1.Description = skill.Description;
            skill1.SkillCategoryId = skill.Category_Skill;
            var a = _RepoSkills.AddSkill(skill1);
            if(a != null)
            {
                return Ok();
            }
            return BadRequest("Error");
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] DtoSkill skill)
        {
            var a = _RepoSkills.UpdateSkill(id, skill);
            if(a != null)
            {
                return Ok();
            }
            return BadRequest("id : "+id+" n'est pas trouvable");

            
        }


        [HttpDelete]
        public async Task<IActionResult> RemoveSkill(int id)
        {
            var a = _RepoSkills.RemoveSkill(id);
            if (a != null)
            {
                return Ok("Removed Succefully");
            }
            return BadRequest("id : " + id + " n'est pas trouvable");


        }
    }
}
