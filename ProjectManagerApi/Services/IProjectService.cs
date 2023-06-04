using ProjectManagerApi.Data.Models;

namespace ProjectManagerApi.Services
{
    public interface IProjectService
    {
        Task<Project> CreateProject(int userId, Project project, List<int> technologies, List<int> languages);
        Task<List<Project>> GetAllProjects();
        Task AddUserToProject(int projectId, int leaderId, int userId, int roleId);
        Task ChangeUserRole(int projectId, int leaderId, int userId, int roleId);
        Task SetProjectStatus(int projectId, int leaderId, int statusId);
        Task<IEnumerable<Project>> GetAllProjectWithPrivateRecruitment();
    }
}