using System.Linq;
using System;
using System.Data.Objects.DataClasses;
namespace RepoMan
{
    public interface IRepository<TContext> where TContext : new()
    {
        IQueryable<TRepository> Where<TRepository>(Func<TRepository, bool> query) where TRepository : EntityObject;
        void Save<TRepository>(TRepository entity) where TRepository : EntityObject;
        void Delete<TRepository>(TRepository entity) where TRepository : EntityObject;
    }
}
