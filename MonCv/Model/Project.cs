using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MonCv.Model
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [MaxLength(500),Required]
        public string Details { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[] Image { get; set; }
        public List<Skills> SkillsUsed { get; set; }
        [JsonIgnore]
        public List<project_skill> project_Skills { get; set; }        
    }
}
