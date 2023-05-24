using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Data.Repositories;
using ProjectManagerApi.Exceptions;
using ProjectManagerApi.Extensions;

namespace ProjectManagerApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IBaseRepository<Project, int> projectRepository;
        private readonly IBaseRepository<User, int> userRepository;
        private readonly IBaseRepository<Tech, int> technologyRepsitory;
        private readonly IBaseRepository<Language, int> languageRepository;
        private readonly IBaseRepository<Team, int> teamRepository;
        private readonly IBaseRepository<TeamUser, (int userId, int teamId, int RoleId)> teamUserRepository;
        private readonly IBaseRepository<Role, int> roleRepository;

        public ProjectService(
            IBaseRepository<Project, int> projectRepository,
            IBaseRepository<User, int> userRepository,
            IBaseRepository<Tech, int> technologyRepsitory,
            IBaseRepository<Language, int> languageRepository,
            IBaseRepository<Team, int> teamRepository,
            IBaseRepository<TeamUser, (int userId, int teamId, int RoleId)> teamUserRepository,
            IBaseRepository<Role, int> roleRepository)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.technologyRepsitory = technologyRepsitory;
            this.languageRepository = languageRepository;
            this.teamRepository = teamRepository;
            this.teamUserRepository = teamUserRepository;
            this.roleRepository = roleRepository;
        }

        public async Task<List<Project>> GetAllProjects()
        {
            return await projectRepository.GetAll();
        }

        public async Task<Project> CreateProject(int userId, Project project, List<int> technologies, List<int> languages)
        {
            var user = await userRepository.Get(userId);
            if (user == null)
            {
                throw new InvalidItemIdException("User not found");
            }

            var projectTechnologies = await technologyRepsitory.GetCollectionFromDB(technologies, "Technology not found");
            var projectLanguages = await languageRepository.GetCollectionFromDB(languages, "Language not found");

            var team = new Team { Name = project.Name + " - team" };

            var role = await roleRepository.FindFirst(role => role.Name == "Leader");
            if(role == null)
            {
                throw new InvalidItemIdException("Role 'Leader' not found");
            }

            var teamUser = new TeamUser
            {
                Role = role,
                Team = team,
                User = user
            };

            project.Team = team;
            project.Technologies.AddRange(projectTechnologies);
            project.Languages.AddRange(projectLanguages);

            await teamRepository.Add(team);
            await teamUserRepository.Add(teamUser);
            await projectRepository.Add(project);

            return project;
        }
    }
}
