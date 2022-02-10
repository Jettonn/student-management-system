using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SMS.Application.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Add(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
        T GetSingleByCriteria(Expression<Func<T, bool>> criteria);
        public List<T> ListByCriteria(Expression<Func<T, bool>> criteria, params string[] includes);
    }
}
