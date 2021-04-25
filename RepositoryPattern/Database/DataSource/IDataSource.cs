using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RepositoryPattern.Models;

namespace RepositoryPattern.Database.DataSource
{
    public interface IDataSource : IDisposable
    {
        public T FindById<T>(Guid id) where T : BaseEntity;
        IEnumerable<T> FindAll<T>() where T : BaseEntity;
        IEnumerable<T> FindByCondition<T>(Expression<Func<T, bool>> expression) where T : BaseEntity;
        T FindFirstByCondition<T>(Expression<Func<T, bool>> expression) where T : BaseEntity;

        T Add<T>(T entity) where T : BaseEntity;
        T Update<T>(T entity) where T : BaseEntity;
        T Remove<T>(T entity) where T : BaseEntity;
        bool SaveChanges();
    }
}