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
        private readonly Dictionary<Type, HashSet<object>> _repositories = new Dictionary<Type, HashSet<object>>();

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
            try
            {
                return _repositories[typeof(TRepository)].Cast<TRepository>().Where(query.Compile()).AsQueryable();
            }
            catch (Exception)
            {
                return new List<TRepository>().AsQueryable();
            }
        }

        public IQueryable<TRepository> Where<TRepository>(Expression<Func<TRepository, bool>> query, Expression<Func<TRepository, dynamic>> columns) where TRepository : EntityObject
        {
            try
            {
                IQueryable<dynamic> dynamics = _repositories[typeof(TRepository)].Cast<TRepository>().Where(query.Compile()).AsQueryable().Select(columns);
                return DynamicToStatic.ToStatic<TRepository>(dynamics);
            }
            catch (Exception)
            {
                return new List<TRepository>().AsQueryable();
            }
        }

        public void Save<TRepository>(TRepository entity) where TRepository : EntityObject
        {
            if (_repositories.ContainsKey(typeof(TRepository)))
                _repositories[typeof(TRepository)].Add(entity);
            else
                _repositories.Add(typeof(TRepository), new HashSet<object> { entity });
        }

        public void Delete<TRepository>(TRepository entity) where TRepository : EntityObject
        {
            if (_repositories.ContainsKey(typeof(TRepository)))
                _repositories[typeof(TRepository)].Remove(entity);
        }

        public void InitializeRepository<TRepository>()
        {
            if (!_repositories.ContainsKey(typeof(TRepository)))
                _repositories.Add(typeof(TRepository), new HashSet<object>());
        }


        public T ExecuteRawSql<T>(Func<SqlConnection, T> sqlFunction)
        {
            return sqlFunction(new SqlConnection());
        }
    }
}