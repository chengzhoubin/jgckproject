﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using JGCK.Util.Enums;

namespace JGCK.Framework.EF
{
    public abstract class AbstractUnitOfWork : DbContext, IDBProxy
    {
        protected AbstractUnitOfWork()
        {
        }

        protected AbstractUnitOfWork(string aliasName) : base(aliasName)
        {
            //base.Configuration.
        }

        public virtual TEntity GetById<TEntity, TPKey>(TPKey pkey) where TEntity : class
        {
            var ret = this.Set<TEntity>().Find(pkey);
            return ret;
        }

        public virtual Task<TEntity> GetByIdAsync<TEntity, TPKey>(TPKey pkey) where TEntity : class
        {
            var ret = this.Set<TEntity>().FindAsync(pkey);
            return ret;
        }

        public virtual IEnumerable<TEntity> GetObjects<TEntity>(
            Expression<Func<TEntity, bool>> exp,
            Pager p = null,
            bool withTracking = true,
            OrderByExpression<TEntity>[] orderByExpressions = null,
            params Expression<Func<TEntity, object>>[] includeExpressions)
            where TEntity : class
        {
            var entitySet = this.Set<TEntity>();
            if (includeExpressions != null)
            {
                foreach (var includeExp in includeExpressions)
                {
                    entitySet.Include(includeExp);
                }
            }

            var ret = entitySet.Where(exp);
            if (orderByExpressions != null)
            {
                ret = ret.Sort(orderByExpressions);
            }

            if (p != null)
                ret = ret.Skip((p.CurrentIndex - 1) * p.PageSize).Take(p.PageSize);
            if (!withTracking)
                ret = ret.AsNoTracking();

            return ret;
        }

        public virtual Task<List<TEntity>> GetObjectsAsync<TEntity>(
            Expression<Func<TEntity, bool>> exp,
            Pager p = null,
            bool withTracking = true,
            OrderByExpression<TEntity>[] orderByExpressions = null,
            params Expression<Func<TEntity, object>>[] includeExpressions)
            where TEntity : class
        {
            var ret = (IQueryable<TEntity>) GetObjects(exp, p, withTracking, orderByExpressions, includeExpressions);
            return ret.ToListAsync();
        }

        public int Add<TEntity>(TEntity ent) where TEntity : class
        {
            this.Set<TEntity>().Add(ent);
            return this.SaveChanges();
        }

        public Task<int> AddAsync<TEntity>(TEntity ent) where TEntity : class
        {
            this.Set<TEntity>().Add(ent);
            return this.SaveChangesAsync();
        }

        public int Delete<TEntity>(Expression<Func<TEntity, bool>> deleteExpression) where TEntity : class
        {
            return this.Set<TEntity>().Where(deleteExpression).Delete();
        }

        public Task<int> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> deleteExpression) where TEntity : class
        {
            return this.Set<TEntity>().Where(deleteExpression).DeleteAsync();
        }

        public int Commit()
        {
            return this.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return this.SaveChangesAsync();
        }

        public class Pager
        {
            public int CurrentIndex { get; set; } = 1;

            public int PageSize { get; set; } = 10;
        }

        public class OrderByExpression<TEntity> where TEntity : class
        {
            public string OrderByExpressionMember { get; set; }

            public AscOrDesc SortBy { get; set; }
        }
    }
}
