using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoMan
{
    public class TestRepository<TContext> : IRepository<TContext> where TContext : new()
    {
        private Dictionary<Type, List<object>> _repositories = new Dictionary<Type, List<object>>();

        public IQueryable<TRepository> Where<TRepository>(Func<TRepository, bool> query)
        {
            return _repositories[typeof(TRepository)].Cast<TRepository>().Where(query).AsQueryable();
        }

        public void Save<TRepository>(TRepository entity)
        {
            if (_repositories.ContainsKey(typeof(TRepository)))
                _repositories[typeof(TRepository)].Add(entity);
            else
                _repositories.Add(typeof(TRepository), new List<object> { entity });
        }

        public void Delete<TRepository>(TRepository entity)
        {
            throw new NotImplementedException();
        }

        public void InitializeRepository<TRepository>()
        {
            if (!_repositories.ContainsKey(typeof(TRepository)))
                _repositories.Add(typeof(TRepository), new List<object>());
        }
    }
}
