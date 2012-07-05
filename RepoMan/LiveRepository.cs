﻿using System;
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
        ObjectContext _context = new TContext() as ObjectContext;
        Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public IQueryable<TRepository> Where<TRepository>(Expression<Func<TRepository, bool>> query) where TRepository : EntityObject
        {
            if (_repositories.ContainsKey(typeof(TRepository)))
                return ((ObjectSet<TRepository>)_repositories[typeof(TRepository)]).Where(query);
            else
            {
                var objectSet = _context.CreateObjectSet<TRepository>();
                _repositories.Add(typeof(TRepository), objectSet);
                return ((ObjectSet<TRepository>)_repositories[typeof(TRepository)]).Where(query);
            }
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
                    var objectSet = _context.CreateObjectSet<TRepository>();
                    _repositories.Add(typeof(TRepository), objectSet);


                    objectSet.AddObject(entity);
                }
            }

            _context.SaveChanges();
        }

        public void Delete<TRepository>(TRepository entity) where TRepository : EntityObject
        {
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
    }
}