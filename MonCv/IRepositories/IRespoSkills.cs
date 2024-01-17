using MonCv.Dtos;
using MonCv.Model;

namespace MonCv.IRepositories
{
    public interface IRespoSkills
    {

        public Task<List<Skills>> GetAllSkills();
        public Skills? GetSkillsById(int id);
        public List<Skills> GetSkillsByName(string Name);

        public Task AddSkill(Skills skill);
        public string UpdateSkill(int id, DtoSkill skill);
        public Task RemoveSkill(int id);
    }
}
