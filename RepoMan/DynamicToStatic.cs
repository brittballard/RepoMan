using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RepoMan
{
    public static class DynamicToStatic
    {
        public static T ToStatic<T>(dynamic source)
        {
            var target = Activator.CreateInstance<T>();

            IEnumerable<string> properties = GetMemberNames(source);
            if (properties == null)
                return target;

            foreach (string property in properties)
            {
                var propertyInfoSource = source.GetType().GetProperty(property);
                var propertyInfoTarget = target.GetType().GetProperty(property);
                if (propertyInfoTarget != null)
                {
                    object value = propertyInfoSource.GetValue(source, null);
                    if(value != null)
                        propertyInfoTarget.SetValue(target, value, null);
                }
            }
            return target;
        }

        public static IQueryable<T> ToStatic<T>(IQueryable<dynamic> dynamics)
        {
            var objects = new List<T>();
            var list = dynamics.ToList();
            foreach (var d in list)
                objects.Add(DynamicToStatic.ToStatic<T>(d));
            return objects.AsQueryable();
        }

        /// <summary>
        /// Gets the member names of properties. Not all IDynamicMetaObjectProvider have support for this.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="dynamicOnly">if set to <c>true</c> [dynamic only]. Won't add reflected properties</param>
        /// <returns></returns>
        private static IEnumerable<string> GetMemberNames(object target, bool dynamicOnly = false)
        {
            var tList = new List<string>();
            if (!dynamicOnly)
            {
                tList.AddRange(target.GetType().GetProperties().Select(it => it.Name));
            }

            var tTarget = target as IDynamicMetaObjectProvider;
            if (tTarget != null)
            {
                tList.AddRange(tTarget.GetMetaObject(Expression.Constant(tTarget)).GetDynamicMemberNames());
            }
            return tList;
        } 
    }

}
