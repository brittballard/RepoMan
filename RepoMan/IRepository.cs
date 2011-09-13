using System.Linq;
using System;
namespace RepoMan
{
    public interface IRepository<TContext, TRepository> where TContext : new()
    {
        IQueryable<TRepository> Where(Func<TRepository,bool> query);
        void Save(TRepository entity);
        void Delete(TRepository entity);
    }
}
