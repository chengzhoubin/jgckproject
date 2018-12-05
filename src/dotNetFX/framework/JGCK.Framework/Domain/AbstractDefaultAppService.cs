using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JGCK.Framework.Repository;

namespace JGCK.Framework
{
    public class AbstractDefaultAppService : IAppService
    {
        private IList<IDBProxy> proxyContrainer;

        public Func<object, bool> PreLogicDeleteHandler { get; set; }
        public Func<bool> PreOnAddHandler { get; set; }
        public Func<bool> PreOnUpdateHandler { get; set; }

        public AbstractDefaultAppService()
        {
            proxyContrainer = new List<IDBProxy>();
            this.GetModuleInService();
        }

        protected virtual void GetModuleInService()
        {
            var typeOfService = this.GetType();
            var propsInTypeInfos = typeOfService.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            var specPropsType =
                propsInTypeInfos.Where(p => p.PropertyType.GetInterface(typeof(IDBProxy).FullName) != null);
            if (!specPropsType.Any())
            {
                return;
            }

            var spec = specPropsType.Select(p =>
            {
                var propImplement = p.PropertyType.Assembly.CreateInstance(p.PropertyType.FullName);
                p.SetValue(this, propImplement);
                proxyContrainer.Add((IDBProxy) propImplement);
                return propImplement;
            });
            CallContext.SetData(string.Format(HostVer.IDBProxy_Slot_Format, this.GetType().Name), spec.ToList());
        }

        public void Dispose()
        {
            if (proxyContrainer.Count == 0)
                return;
            ((List<IDBProxy>) proxyContrainer).ForEach(act => act.Dispose());
        }

        public virtual Task<int> LogicObjectDelete<TEntity, T>(T pkId, bool isAsync = false)
            where TEntity : class
        {
            var objectContext = GetObjectContextDynamical<TEntity>();
            var entObject = objectContext.GetById<TEntity, T>(pkId);
            var canExcuteDeleted = PreLogicDeleteHandler?.Invoke(entObject);
            if (canExcuteDeleted != null && !canExcuteDeleted.Value)
            {
                return Task.FromResult(0);
            }
            if (entObject == null)
            {
                throw new NullReferenceException();
            }

            entObject.IsDeleted = true;
            var entDbProxy = (IDBProxy) objectContext;
            return isAsync
                ? Task.FromResult(entDbProxy.Commit())
                : entDbProxy.CommitAsync();
        }

        public virtual Task<int> AddObject<TEntity>(TEntity ent, bool isAsync = false) where TEntity : class
        {
            var objectContext = GetObjectContextDynamical<TEntity>();
            var canAdding = PreOnAddHandler?.Invoke();
            if (canAdding.HasValue && canAdding.Value)
            {
                return isAsync ? Task.FromResult(objectContext.Add(ent)) : objectContext.AddAsync(ent);
            }

            return Task.FromResult(0);
        }

        public virtual Task<int> UpdateObject<TEntity>(TEntity ent, bool isAsync = false) where TEntity : class
        {
            var objectContext = GetObjectContextDynamical<TEntity>();
            var canUpdating = PreOnAddHandler?.Invoke();
            if (canUpdating.HasValue && canUpdating.Value)
            {
                var entDbProxy = (IDBProxy) objectContext;
                return isAsync ? Task.FromResult(entDbProxy.Commit()) : entDbProxy.CommitAsync();
            }
            return Task.FromResult(0);
        }

        private dynamic GetObjectContextDynamical<TEntity>()
        {
            var typeObjectContext = typeof(TEntity).GetInterface(typeof(IEntity<>).FullName)?.GetGenericArguments()[0];
            if (typeObjectContext == null)
            {
                throw new NotSupportedException("Entity未指定DbContext");
            }

            var propsInTypeInfos = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            var objectContextPropertyInfo =
                propsInTypeInfos.FirstOrDefault(p => p.PropertyType == typeObjectContext);
            var objectContext = (dynamic) objectContextPropertyInfo?.GetValue(this);
            if (objectContext == null)
            {
                throw new NullReferenceException();
            }

            return objectContext;
        }
    }
}
