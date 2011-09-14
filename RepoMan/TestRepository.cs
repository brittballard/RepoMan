using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;

namespace RepoMan
{
    public class TestRepository<TContext> : IRepository<TContext> where TContext : new()
    {
        private Dictionary<Type, List<object>> _repositories = new Dictionary<Type, List<object>>();

        public IQueryable<TRepository> Where<TRepository>(Func<TRepository, bool> query) where TRepository : EntityObject
        {
            return _repositories[typeof(TRepository)].Cast<TRepository>().Where(query).AsQueryable();
        }

        public void Save<TRepository>(TRepository entity) where TRepository : EntityObject
        {
            if (_repositories.ContainsKey(typeof(TRepository)))
                _repositories[typeof(TRepository)].Add(entity);
            else
                _repositories.Add(typeof(TRepository), new List<object> { entity });
        }

        public void Delete<TRepository>(TRepository entity) where TRepository : EntityObject
        {
            if (_repositories.ContainsKey(typeof(TRepository)))
                _repositories[typeof(TRepository)].Remove(entity);
        }

        public void InitializeRepository<TRepository>()
        {
            if (!_repositories.ContainsKey(typeof(TRepository)))
                _repositories.Add(typeof(TRepository), new List<object>());
        }
    }
}
