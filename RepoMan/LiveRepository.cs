using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Data.EntityClient;

namespace RepoMan
{
    public class LiveRepository<TContext> : IRepository<TContext> where TContext : ObjectContext, new()
    {
        private readonly ObjectContext _context = new TContext() as ObjectContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private readonly MergeOption _mergeOption;

        public LiveRepository()
        {}

        public LiveRepository(MergeOption mergeOption)
        {
            _mergeOption = mergeOption;
        }

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
            if (_repositories.ContainsKey(typeof(TRepository)))
                return ((ObjectSet<TRepository>)_repositories[typeof(TRepository)]).Where(query);
            else
            {
                AddObjectSet<TRepository>();
                return ((ObjectSet<TRepository>)_repositories[typeof(TRepository)]).Where(query);
            }
        }

        public IQueryable<TRepository> Where<TRepository>(Expression<Func<TRepository, bool>> query, Expression<Func<TRepository, dynamic>> columns) where TRepository : EntityObject
        {
            IQueryable<dynamic> dynamics;
            if (_repositories.ContainsKey(typeof(TRepository)))
                dynamics = ((ObjectSet<TRepository>)_repositories[typeof(TRepository)]).Where(query).Select(columns);
            else
            {
                AddObjectSet<TRepository>();
                dynamics = ((ObjectSet<TRepository>)_repositories[typeof(TRepository)]).Where(query).Select(columns);
            }

            return DynamicToStatic.ToStatic<TRepository>(dynamics);
        }

        public void Save<TRepository>(TRepository entity) where TRepository : EntityObject
        {
            if (entity.EntityState == System.Data.EntityState.Detached)
            {
                if (_repositories.ContainsKey(typeof(TRepository)))
                {
                    ((ObjectSet<TRepository>)_repositories[typeof(TRepository)]).AddObject(entity);
                }
                else
                {
                    var objectSet = AddObjectSet<TRepository>();
                    objectSet.AddObject(entity);
                }
            }

            _context.SaveChanges();
        }

        public void Delete<TRepository>(TRepository entity) where TRepository : EntityObject
        {
            if (!_repositories.ContainsKey(typeof(TRepository)))
            {
                AddObjectSet<TRepository>();
            }

            ((ObjectSet<TRepository>)_repositories[typeof(TRepository)]).DeleteObject(entity);

            _context.SaveChanges();
        }


        public T ExecuteRawSql<T>(Func<SqlConnection, T> sqlFunction)
        {
            var sqlConnection = (SqlConnection)((EntityConnection)_context.Connection).StoreConnection;
            
            if (sqlConnection.State != System.Data.ConnectionState.Open)
                sqlConnection.Open();
            
            return sqlFunction(sqlConnection);
        }

        private ObjectSet<TRepository> AddObjectSet<TRepository>() where TRepository : EntityObject
        {
            ObjectSet<TRepository> objectSet = _context.CreateObjectSet<TRepository>();
            if(_mergeOption != null)
                objectSet.MergeOption = _mergeOption;
            _repositories.Add(typeof(TRepository), objectSet);
            return objectSet;
        }
    }
}
