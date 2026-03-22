 using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Tasneem_Shop.Entities.Repositories
{
    public interface IGenericRepository<T> where T: class
    {
        
        IEnumerable <T> GetAll(Expression <Func<T,bool>>? predicate= null,string? Includeword= null);

        T GetFirstOrDefault(Expression<Func<T,bool>>? perdicate = null, string? Includeword = null);

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

    }
}
