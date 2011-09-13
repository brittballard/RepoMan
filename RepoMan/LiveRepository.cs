using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoMan
{
    public class LiveRepository<TContext> : IRepository<TContext> where TContext : new()
    {
        public IQueryable<TRepository> Where<TRepository>(Func<TRepository, bool> query)
        {
            throw new NotImplementedException();
        }

        public void Save<TRepository>(TRepository entity)
        {
            throw new NotImplementedException();
        }

        public void Delete<TRepository>(TRepository entity)
        {
            throw new NotImplementedException();
        }
    }
}
