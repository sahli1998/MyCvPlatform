namespace MonCv.Model
{
    public class project_skill
    {
        public int SkillId { get; set; }
        public Skills Skills { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime create_date { get; set; }
    }
}
