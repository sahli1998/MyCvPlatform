using System.Text.Json.Serialization;

namespace MonCv.Model
{
    public class Skills
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SkillCategoryId { get; set; }
        public SkillCategory SkillCategory { get; set; }
        public List<project_skill> project_Skills { get; set; } 
        public List<Project> Projects { get; set; }
    }
}
