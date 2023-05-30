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
            var user = await userRepository.Get(userId) ?? throw new InvalidItemIdException("User not found");

            var projectTechnologies = await technologyRepsitory.GetCollectionFromDB(technologies, "Technology not found");
            var projectLanguages = await languageRepository.GetCollectionFromDB(languages, "Language not found");

            var team = new Team { Name = project.Name + " - team" };

            var role = await roleRepository.FindFirst(role => role.Name == "Leader") ?? throw new InvalidItemIdException("Role 'Leader' not found");

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

        public async Task AddUserToProject(int projectId, int leaderId, int userId, int roleId)
        {
            var leader = await userRepository.Get(leaderId) ?? throw new InvalidItemIdException("Leader not found");
            var user = await userRepository.Get(userId) ?? throw new InvalidItemIdException("User not found"); 
            var project = await projectRepository.Get(projectId) ?? throw new InvalidItemIdException("Project not found"); 
            var role = await roleRepository.Get(roleId) ?? throw new InvalidItemIdException("Role not found");

            var leaderRole = await roleRepository.FindFirst(role => role.Name == "Leader") ?? throw new InvalidItemIdException("Role 'Leader' not found");
            var teamLeader = await teamUserRepository.FindFirst(x => x.RoleId == leaderRole.RoleId && x.TeamId == project.TeamId) ?? throw new InvalidItemIdException("Team leader not found");

            if (teamLeader.UserId != leaderId)
            {
                throw new InvalidItemIdException("User is not a leader of this project");
            }

            if(await teamUserRepository.FindFirst(x => x.UserId == userId && x.TeamId == project.TeamId) != null)
            {
                throw new InvalidItemIdException("User is already in this project");
            }

            var teamUser = new TeamUser
            {
                RoleId = roleId,
                TeamId = project.TeamId,
                UserId = userId
            };
            await teamUserRepository.Add(teamUser);
        }
    }
}
