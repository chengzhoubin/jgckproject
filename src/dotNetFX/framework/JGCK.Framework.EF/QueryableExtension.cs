using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JGCK.Util.Enums;

namespace JGCK.Framework.EF
{
    public static class QueryableExtension
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> collection,
            AbstractUnitOfWork.OrderByExpression<T>[] sortBys) where T : class
        {
            var orderJoinStr = new StringBuilder();
            foreach (var sort in sortBys)
            {
                orderJoinStr.Append(sort.OrderByExpressionMember)
                    .Append(sort.SortBy == AscOrDesc.Asc ? "" : " descending")
                    .Append(",");
            }

            return collection.OrderBy<T>(orderJoinStr.ToString().Trim(','));
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> collection, string sortBy, bool reverse = false)
        {
            return collection.OrderBy<T>(sortBy + (reverse ? " descending" : ""));
        }

        /*
        //public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName)
        //{
        //    return _OrderBy<T>(query, propertyName, false);
        //}
        //public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName)
        //{
        //    return _OrderBy<T>(query, propertyName, true);
        //}

        static IOrderedQueryable<T> _OrderBy<T>(IQueryable<T> query, string propertyName, bool isDesc)
        {
            string methodname = (isDesc) ? "OrderByDescendingInternal" : "OrderByInternal";
            var memberProp = typeof(T).GetProperty(propertyName);
            var method = typeof(QueryableExtension).GetMethod(methodname)
                .MakeGenericMethod(typeof(T), memberProp.PropertyType);

            return (IOrderedQueryable<T>) method.Invoke(null, new object[] {query, memberProp});
        }

        public static IOrderedQueryable<T> OrderByInternal<T, TProp>(IQueryable<T> query, PropertyInfo memberProperty)
        {
            return query.OrderBy(_GetLamba<T, TProp>(memberProperty));
        }

        public static IOrderedQueryable<T> OrderByDescendingInternal<T, TProp>(IQueryable<T> query, PropertyInfo memberProperty)
        {
            return query.OrderByDescending(_GetLamba<T, TProp>(memberProperty));
        }

        public static Expression<Func<T, TProp>> _GetLamba<T, TProp>(this PropertyInfo memberProperty)
        {
            //if (memberProperty.PropertyType != typeof(TProp))
            //    throw new Exception();

            var thisArg = Expression.Parameter(typeof(T));
            var lamba = Expression.Lambda<Func<T, TProp>>(Expression.Property(thisArg, memberProperty), thisArg);
            return lamba;
        }
        */
    }
}
