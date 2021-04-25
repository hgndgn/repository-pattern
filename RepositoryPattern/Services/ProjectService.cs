using System;
using System.Collections.Generic;
using RepositoryPattern.Database.Repository;
using RepositoryPattern.Database.Repository.Specific;
using RepositoryPattern.Models;

namespace RepositoryPattern.Services

{
    public interface IProjectService
    {
        Project Add(Project project);
        Project Update(Project project);
        Project FindById(Guid id);
        IEnumerable<Project> FindAll();
    }

    public class ProjectService : CommonService<Project>, IProjectService
    {
        private readonly IProjectRepository _repository;

        public ProjectService(IUnitOfWork unitOfWork) : base(unitOfWork.ProjectRepository)
        {
            _repository = unitOfWork.ProjectRepository;
        }
    }
}