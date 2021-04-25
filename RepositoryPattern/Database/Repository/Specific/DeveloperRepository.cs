using RepositoryPattern.Database.DataSource;
using RepositoryPattern.Models;

namespace RepositoryPattern.Database.Repository.Specific
{
    public interface IDeveloperRepository : IRepository<Developer>
    {
    }

    public class DeveloperRepository : Repository<Developer>, IDeveloperRepository
    {
        private readonly IDataSource _dataSource;

        public DeveloperRepository(IDataSource dataSource) : base(dataSource)
        {
            _dataSource = dataSource;
        }
    }
}