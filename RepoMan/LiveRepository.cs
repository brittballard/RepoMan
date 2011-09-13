using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoMan
{
    public class LiveRepository<TContext, TRepository> : IRepository<TContext, TRepository> where TContext : new()
    {
        public IQueryable<TRepository> Where(Func<TRepository, bool> query)
        {
            throw new NotImplementedException();
        }

        public void Save(TRepository entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TRepository entity)
        {
            throw new NotImplementedException();
        }
    }
}
