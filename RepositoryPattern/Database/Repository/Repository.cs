using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RepositoryPattern.Database.DataSource;
using RepositoryPattern.Models;

namespace RepositoryPattern.Database.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        public T FindById(Guid id);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        T FindFirstByCondition(Expression<Func<T, bool>> expression);
        T Add(T entity);
        T Update(T entity);
        bool Remove(T entity);
    }

    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IDataSource _dataSource;

        protected Repository(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public T FindById(Guid id)
        {
            return _dataSource.FindById<T>(id);
        }

        public T FindFirstByCondition(Expression<Func<T, bool>> expression)
        {
            return _dataSource.FindByCondition(expression).FirstOrDefault();
        }

        public IEnumerable<T> FindAll()
        {
            return _dataSource.FindAll<T>();
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _dataSource.FindByCondition(expression);
        }

        public T Add(T entity)
        {
            _dataSource.Add(entity);
            _dataSource.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            _dataSource.Update(entity);
            _dataSource.SaveChanges();
            return entity;
        }

        public bool Remove(T entity)
        {
            _dataSource.Remove(entity);
            return _dataSource.SaveChanges();
        }
    }
}