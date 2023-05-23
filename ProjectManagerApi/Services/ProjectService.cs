using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Data.Repositories;
using ProjectManagerApi.Exceptions;
using ProjectManagerApi.Extensions;

namespace ProjectManagerApi.Services
{
    public class ProjectService
    {
        private readonly IBaseRepository<Project> projectRepository;
        private readonly IBaseRepository<User> userRepository;
        private readonly IBaseRepository<Tech> technologyRepsitory;
        private readonly IBaseRepository<Language> languageRepository;
        private readonly IBaseRepository<Team> teamRepository;

        public ProjectService(
            IBaseRepository<Project> projectRepository, 
            IBaseRepository<User> userRepository,
            IBaseRepository<Tech> technologyRepsitory,
            IBaseRepository<Language> languageRepository,
            IBaseRepository<Team> teamRepository)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.technologyRepsitory = technologyRepsitory;
            this.languageRepository = languageRepository;
            this.teamRepository = teamRepository;
        }

        public async Task<List<Project>> GetAllProjects()
        {
            return await projectRepository.GetAll();
        }

        public async Task<Project> CreateProject(int userId, Project project, List<int> technologies, List<int> languages)
        {
            var user = await userRepository.Get(userId);
            if(user == null)
            {
                throw new InvalidItemIdException("User not found"); 
            }

            var projectTechnologies = await technologyRepsitory.GetCollectionFromDB(technologies, "Technology not found");
            var projectLanguages = await languageRepository.GetCollectionFromDB(languages, "Language not found");

            var team = new Team { Name = project.Name + " - team" };
            team = await teamRepository.Add(team);

            //var projectTeam = new ProjectTeam { Project = project, Team = team }; 

            project.Team = team;
            project.Technologies.AddRange(projectTechnologies);
            project.Languages.AddRange(projectLanguages);   
            await projectRepository.Add(project);

            return project;
        }
    }
}
