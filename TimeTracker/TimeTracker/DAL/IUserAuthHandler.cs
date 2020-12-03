using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TimeTracker.DAL.Models;

namespace TimeTracker.DAL
{
    public interface IUserAuthHandler<TEntity, TKey>
        where TEntity : class
    {
        CreateAccountResult Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TKey id);

        TEntity FindById(TKey id);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    }
}
