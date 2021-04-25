using System;
using System.Collections.Generic;
using RepositoryPattern.Database.Repository;
using RepositoryPattern.Models;

namespace RepositoryPattern.Services
{
    public class CommonService<T> where T : BaseEntity
    {
        private readonly IRepository<T> _repository;

        public CommonService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual T Add(T entity)
        {
            return _repository.Add(entity);
        }

        public virtual T Update(T entity)
        {
            return _repository.Update(entity);
        }

        public virtual T FindById(Guid id)
        {
            return _repository.FindById(id);
        }
        
        public virtual IEnumerable<T> FindAll()
        {
            return _repository.FindAll();
        }
    }
}