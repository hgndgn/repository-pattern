using System;
using System.Collections.Generic;
using RepositoryPattern.Database.Repository;
using RepositoryPattern.Database.Repository.Specific;
using RepositoryPattern.Models;

namespace RepositoryPattern.Services

{
    public interface IDeveloperService
    {
        Developer Add(Developer developer);
        Developer Update(Developer developer);
        Developer FindById(Guid id);
        IEnumerable<Developer> FindAll();
    }

    public class DeveloperService : CommonService<Developer>, IDeveloperService
    {
        private readonly IDeveloperRepository _repository;

        public DeveloperService(IUnitOfWork unitOfWork) : base(unitOfWork.DeveloperRepository)
        {
            _repository = unitOfWork.DeveloperRepository;
        }
    }
}