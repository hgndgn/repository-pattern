using System;
using RepositoryPattern.Database.DataSource;
using RepositoryPattern.Database.Repository.Specific;

namespace RepositoryPattern.Database.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IDeveloperRepository DeveloperRepository { get; }
        IProjectRepository ProjectRepository { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataSource _dataSource;

        public IDeveloperRepository DeveloperRepository { get; }
        public IProjectRepository ProjectRepository { get; }

        public UnitOfWork(IDataSource dataSource)
        {
            _dataSource = dataSource;

            DeveloperRepository = new DeveloperRepository(_dataSource);
            ProjectRepository = new ProjectRepository(_dataSource);
        }

        public void Dispose()
        {
            _dataSource.Dispose();
        }
    }
}