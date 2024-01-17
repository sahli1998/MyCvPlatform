using MonCv.Dtos;
using MonCv.Model;
using MonCv.Repositories;

namespace MonCv.IRepositories
{
    public interface IRepoProject
    {
        public Task<List<ProjectDTO>> GetAllProjects();
        public Task<ProjectDTO?> GetProjectById(int id);
        public List<Project> GetProkjectByName(string Name);

        public Task AddNewProject(ProjectAddDTO Project);

        public Task AddProject(ProjectFormData Project);
        public Task UpdateProject(int id, ProjectAddDTO Project);
        public Task RemoveProject(int id);
    }
    public class  ProjectFormData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IFormFile Image { get; set; }

        public string ListIdSkills { get; set; }

        public string Details { get; set; }
    }
}
