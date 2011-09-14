using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace RepoMan
{
    public class LiveRepository<TContext> : IRepository<TContext> where TContext : ObjectContext, new()
    {
        ObjectContext _context = new TContext() as ObjectContext;
        Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public IQueryable<TRepository> Where<TRepository>(Func<TRepository, bool> query) where TRepository : EntityObject
        {
            return ((ObjectSet<TRepository>)_repositories[typeof(TRepository)]).Where(query).AsQueryable();
        }

        public void Save<TRepository>(TRepository entity) where TRepository : EntityObject
        {
            if (_repositories.ContainsKey(typeof(TRepository)))
                ((ObjectSet<TRepository>)_repositories[typeof(TRepository)]).AddObject(entity);
            else
            {
                var objectSet = _context.CreateObjectSet<TRepository>();
                objectSet.AddObject(entity);
                _repositories.Add(typeof(TRepository), objectSet);
            }

            _context.SaveChanges();
        }

        public void Delete<TRepository>(TRepository entity) where TRepository : EntityObject
        {
            throw new NotImplementedException();
        }
    }
}
