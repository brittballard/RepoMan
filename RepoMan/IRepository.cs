using System.Linq;
using System;
namespace RepoMan
{
    public interface IRepository<TContext> where TContext : new()
    {
        IQueryable<TRepository> Where<TRepository>(Func<TRepository, bool> query);
        void Save<TRepository>(TRepository entity);
        void Delete<TRepository>(TRepository entity);
    }
}
