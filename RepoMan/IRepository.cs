using System.Linq;
using System;
using System.Data.Objects.DataClasses;
using System.Linq.Expressions;
using System.Data.SqlClient;
namespace RepoMan
{
    public interface IRepository<TContext> where TContext : new()
    {
        IQueryable<TRepository> Where<TRepository>(Expression<Func<TRepository, bool>> query) where TRepository : EntityObject;
        void Save<TRepository>(TRepository entity) where TRepository : EntityObject;
        void Delete<TRepository>(TRepository entity) where TRepository : EntityObject;
        T ExecuteRawSql<T>(Func<SqlConnection, T> sqlFunction);
    }
}
