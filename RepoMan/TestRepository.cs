using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace RepoMan
{
    public class TestRepository<TContext> : IRepository<TContext> where TContext : new()
    {
        private readonly Dictionary<Type, List<object>> _repositories = new Dictionary<Type, List<object>>();

        public TRepository FirstOrDefault<TRepository>(Expression<Func<TRepository, bool>> query) where TRepository : EntityObject
        {
            return Where(query).FirstOrDefault();
        }

        public TRepository FirstOrDefault<TRepository>(Expression<Func<TRepository, bool>> query, Expression<Func<TRepository, dynamic>> columns) where TRepository : EntityObject
        {
            return Where(query, columns).FirstOrDefault();
        }

        public IQueryable<TRepository> Where<TRepository>(Expression<Func<TRepository, bool>> query) where TRepository : EntityObject
        {
            return _repositories[typeof(TRepository)].Cast<TRepository>().Where(query.Compile()).AsQueryable();
        }

        public IQueryable<TRepository> Where<TRepository>(Expression<Func<TRepository, bool>> query, Expression<Func<TRepository, dynamic>> columns) where TRepository : EntityObject
        {
            IQueryable<dynamic> dynamics = _repositories[typeof(TRepository)].Cast<TRepository>().Where(query.Compile()).AsQueryable().Select(columns);
            return DynamicToStatic.ToStatic<TRepository>(dynamics);
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


        public T ExecuteRawSql<T>(Func<SqlConnection, T> sqlFunction)
        {
            return sqlFunction(new SqlConnection());
        }
    }
}