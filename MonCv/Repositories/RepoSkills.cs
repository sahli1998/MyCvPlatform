using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MonCv.Data;
using MonCv.Dtos;
using MonCv.IRepositories;
using MonCv.Model;

namespace MonCv.Repositories
{
    public class RepoSkills : IRespoSkills
    {
        private readonly ApplicationDbContext _db;
        public RepoSkills(ApplicationDbContext db)
        {
            _db = db;
        }
        



        public async Task<List<Skills>> GetAllSkills()
        {
            return  _db.Skills.Include(p => p.SkillCategory).ToList();
        }

        public Skills? GetSkillsById(int id)
        {
            
            return  _db.Skills.Include(p => p.SkillCategory).SingleOrDefault(p => p.Id == id);
        }

        public List<Skills> GetSkillsByName(string Name1)
        {
            return _db.Skills.Include(p => p.SkillCategory).Where(p => p.Name.ToLower().Contains(Name1.ToLower())).ToList();
        }

        public Task AddSkill(Skills skills)
        {
            try
            {
                _db.Add(skills);
                _db.SaveChanges();
                return Task.CompletedTask;

            }
            catch(Exception ex)
            {
                return null;

            }
           
            
        }

       

        public Task RemoveSkill(int id)
        {
            var a = _db.Skills.SingleOrDefault(p => p.Id == id);
            if (a != null)
            {
                _db.Remove(a);
                _db.SaveChanges();
                return Task.CompletedTask;
            }
            return null;

        }

        public string UpdateSkill(int id ,DtoSkill skill)
        {
            var a = _db.Skills.SingleOrDefault(p => p.Id == id);
            if (a != null)
            {
                a.Name = skill.Name;
                a.Description = skill.Description;
                a.SkillCategoryId = skill.Category_Skill;
                _db.SaveChanges();
                return "Good";

            }
            return null;
        }
    }
}
