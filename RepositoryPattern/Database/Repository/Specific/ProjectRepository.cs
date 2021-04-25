using RepositoryPattern.Database.DataSource;
using RepositoryPattern.Models;

namespace RepositoryPattern.Database.Repository.Specific
{
    public interface IProjectRepository : IRepository<Project>
    {
    }

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly IDataSource _dataSource;

        public ProjectRepository(IDataSource dataSource) : base(dataSource)
        {
            _dataSource = dataSource;
        }
    }
}