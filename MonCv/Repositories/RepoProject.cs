using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using MonCv.Data;
using MonCv.Dtos;
using MonCv.IRepositories;
using MonCv.Model;
using Newtonsoft.Json;

namespace MonCv.Repositories
{
    public class RepoProject : IRepoProject
    {
        private readonly ApplicationDbContext _db;
        public RepoProject(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddNewProject(ProjectAddDTO projectDTO)
        {
            var newProject = new Project
            {
                Name = projectDTO.Name,
                Description = projectDTO.Description,
                StartDate= projectDTO.StartDate,
                EndDate= projectDTO.EndDate,
                Image=projectDTO.Image,
                // Map other properties as needed
            };
            newProject.SkillsUsed = new List<Skills>();

            foreach (int item in projectDTO.ListIdSkills)
            {
                Skills? a=  _db.Skills.FirstOrDefault(x => x.Id==item);
                if (a!=null)
                {
                    newProject.SkillsUsed.Add(a);
                }
            }
            _db.Projects.Add(newProject);
            _db.SaveChanges();


            /* // Map SkillsDTO to Skills entities
             var skills = projectDTO.SkillsUsed.Select(skillsDTO => new Skills
             {
                 Name = skillsDTO.Name,
                 Description = skillsDTO.Description,
                 // Map other properties as needed
             }).ToList();

             // Create project_skills entities for the association between the project and skills
             var projectSkills = skills.Select(skill => new project_skill
             {
                 Skills = skill,
                 Project = newProject,
                 //create_date = skill.create_date // Assuming create_date is also set for project_skills
             }).ToList();

             // Set the SkillsUsed property of the newProject
             newProject.SkillsUsed = skills;

             // Add entities to the database and save changes
              _db.Projects.Add(newProject);
              _db.project_skill.AddRange(projectSkills);
              _db.SaveChanges();*/

        }

        public async Task AddProject(ProjectFormData Project)
        {
            Project pr = new Project
            {
                Name= Project.Name,
                Description= Project.Description,
                StartDate= Project.StartDate,
                EndDate= Project.EndDate,
                Details= Project.Details,
                

            };
             pr.Image = null;


            //******************************************

            string inputString = Project.ListIdSkills;

            // Divisez la chaîne en sous-chaînes basées sur la virgule
            string[] idStrings = inputString.Split(',');

            // Convertissez chaque sous-chaîne en entier
            List<int> listIds = new List<int>();
            pr.SkillsUsed = new List<Skills>();
            foreach (string idString in idStrings)
            {
                if (int.TryParse(idString, out int id))
                {
                    listIds.Add(id);
                }
                else
                {
                    Console.WriteLine($"La conversion de '{idString}' en entier a échoué.");
                }
            }

            //******************************************

            foreach (int item in listIds)
            {
                Skills? a = _db.Skills.FirstOrDefault(x => x.Id == item);
                if (a != null)
                {
                    pr.SkillsUsed.Add(a);
                }
            }

            using (var ms = new MemoryStream())
            {
                 Project.Image.CopyTo(ms);
                pr.Image = ms.ToArray();
            }
            _db.Projects.Add(pr);
            _db.SaveChanges();
        }

        public async Task<List<ProjectDTO>> GetAllProjects()
        {
            var projects = await _db.Projects
       .Select(p => new ProjectDTO
       {
           Id = p.Id,
           Name = p.Name,
           Description = p.Description,
           StartDate= p.StartDate,
           EndDate= p.EndDate,
           Image= Convert.ToBase64String(p.Image),
           
           SkillsUsed = p.project_Skills.Select(ps => new SkillsDTO
           {
               Id = ps.Skills.Id,
               Name = ps.Skills.Name,
               Description = ps.Skills.Description,
               create_date = ps.create_date
           }).ToList()
       })
       .ToListAsync();

            return projects;
        }

        public async Task<ProjectDTO?> GetProjectById(int id)
        {
            var projects =  _db.Projects
       .Select(p => new ProjectDTO
       {
           Id = p.Id,
           Name = p.Name,
           Description = p.Description,
           StartDate = p.StartDate,
           EndDate = p.EndDate,
           Details= p.Details,
           Image = Convert.ToBase64String(p.Image),

           SkillsUsed = p.project_Skills.Select(ps => new SkillsDTO
           {
               Id = ps.Skills.Id,
               Name = ps.Skills.Name,
               Description = ps.Skills.Description,
               create_date = ps.create_date
           }).ToList()
       }).FirstOrDefault(p => p.Id == id);
      

            return projects;

        }

        public List<Project> GetProkjectByName(string Name)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveProject(int id)
        {
            Project? project = _db.Projects.FirstOrDefault(p => p.Id == id);
            if(project != null)
            {

                _db.Projects.Remove(project);
                
            }
            _db.SaveChanges();
        }

        public async Task UpdateProject(int id, ProjectAddDTO Project)
        {
            List<int> List_Current_Skills = new List<int>();
            List<int> List_New_Skills = new List<int>();
            List_New_Skills = Project.ListIdSkills.ToList();
            Project? a =   _db.Projects.Include(p => p.project_Skills).FirstOrDefault(x => x.Id == id);
            if(a != null)
            {
                 List_Current_Skills = a.project_Skills.Select(p => p.SkillId).ToList();
                List<int> List_RemovedSkills = List_Current_Skills.Except(List_New_Skills).ToList();
                foreach (int item in List_RemovedSkills)
                {

                    project_skill? b = _db.project_skill.FirstOrDefault(p => p.SkillId == item && p.ProjectId == id);
                    if (b != null)
                    {
                        _db.project_skill.Remove(b);

                    }

                }
                a.SkillsUsed = new List<Skills>();
                foreach (int item1 in List_New_Skills)
                {

                    Skills? pr = _db.Skills.FirstOrDefault(x => x.Id == item1);
                    
                    if (pr != null)
                    {
                        a.SkillsUsed.Add(pr);
                    }





                }


                List<int> List_AddedSkills = List_New_Skills.Except(List_Current_Skills).ToList();

                a.Description = Project.Description;
                a.Name = Project.Name;
                a.StartDate = Project.StartDate;
                a.EndDate = Project.EndDate;
                a.Image=Project.Image;
                _db.SaveChanges();

            }
          






        }
    }
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Image { get; set; }
        public List<SkillsDTO> SkillsUsed { get; set; }
    }
    public class ProjectAddDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Details { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public byte[] Image { get; set; }
        public List<int> ListIdSkills { get; set; }
    }

    public class SkillsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Details { get; set; }


        public DateTime create_date { get; set; }
    }
    public class ProjectSkillDTO
    {
       public int ProjectId { get; set; }
        public int SkillId { get; set; }
    }
}
