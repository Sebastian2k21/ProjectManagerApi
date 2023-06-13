using ProjectManagerApi.Data.Models;
using System.Runtime.CompilerServices;

namespace ProjectManagerApi.Services
{
    public interface IProjectService
    {
        Task<Project> CreateProject(int userId, Project project, List<int> technologies, List<int> languages);
        Task<Project> UpdateProject(int userId, int projectId, Project project);
        Task<Project > GetProjectById(int projectId);
        Task ApplyUserToProject(int userId, int projectId);
        Task<List<Project>> GetAllProjects();
        Task AddUserToProject(int projectId, int leaderId, int userId, int roleId);
        Task ChangeUserRole(int projectId, int leaderId, int userId, int roleId);
        Task SetProjectStatus(int projectId, int leaderId, int statusId);
        Task<IEnumerable<Project>> GetAllProjectWithPrivateRecruitment();
        Task<IEnumerable<Project>> GetProjectsByLanguage(int langId);
        Task<IEnumerable<Project>> GetProjectsByTech(int techId);
        Task<IEnumerable<User>> GetApplicants(int leaderId, int projectId);
        Task SetRepositoryUrl(int userId, int projectId, string repositoryUrl);
        Task<IEnumerable<TeamUser>> GetProjectMembers(int projectId);
        Task<IEnumerable<User>> GetAllUsers();
    }
}