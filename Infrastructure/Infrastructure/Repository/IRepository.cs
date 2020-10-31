using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        T GetById(Guid id);

        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        IEnumerable<T> GetAll();
        int Insert(IEnumerable<T> entities);
        int Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
    }
}
