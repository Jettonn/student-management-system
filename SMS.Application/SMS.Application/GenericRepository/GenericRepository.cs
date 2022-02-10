using Microsoft.EntityFrameworkCore;
using SistemiPerMenaxhiminEVijueshmerise.GenericRepository;
using SMS.Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMS.Application.wwwroot.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private SMSDatabaseContext _context = null;
        private DbSet<T> table = null;
        public GenericRepository()
        {
            this._context = new SMSDatabaseContext();
            table = _context.Set<T>();
        }
        public GenericRepository(SMSDatabaseContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public void Add(T obj)
        {
            table.Add(obj);
            _context.SaveChanges();
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
            _context.SaveChanges();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public T GetSingleByCriteria(Expression<Func<T, bool>> criteria)
        {
            return ListByCriteria(criteria).FirstOrDefault();
        }

        public List<T> ListByCriteria(Expression<Func<T, bool>> criteria, params string[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                int count = includes.Length;
                for (int index = 0; index < count; index++)
                {
                    query = query.Include(includes[index]);
                }
            }

            return query.Where(criteria).ToList();
        }

    }
}
