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

        public Func<bool> PreLogicDeleteHandler { get; set; }
        public Func<bool> PreOnAddHandler { get; set; }
        public Func<object> PreOnUpdateHandler { get; set; }
        public Action<object, object> OnUpdatingHandler { get; set; }

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

        public virtual Task<AppServiceExecuteStatus> LogicObjectDelete<TEntity, T>(T pkId, bool isAsync = false)
            where TEntity : class
        {
            var objectContext = GetObjectContextDynamical<TEntity>();
            var entObject = objectContext.GetById<TEntity, T>(pkId);
            var canExcuteDeleted = PreLogicDeleteHandler?.Invoke();
            if (canExcuteDeleted != null && !canExcuteDeleted.Value)
            {
                return Task.FromResult(AppServiceExecuteStatus.DoNotContinue);
            }
            if (entObject == null)
            {
                throw new NullReferenceException();
            }

            entObject.IsDeleted = true;
            var entDbProxy = (IDBProxy) objectContext;
            var taskExec = isAsync ? Task.FromResult(entDbProxy.Commit()) : entDbProxy.CommitAsync();
            return taskExec.ContinueWith(
                t => t.Result >= 0 ? AppServiceExecuteStatus.Success : AppServiceExecuteStatus.Fail,
                TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public virtual Task<AppServiceExecuteStatus> AddObject<TEntity>(TEntity ent, bool isAsync = false)
            where TEntity : class
        {
            var objectContext = GetObjectContextDynamical<TEntity>();
            var canAdding = PreOnAddHandler?.Invoke();
            if (canAdding.HasValue && canAdding.Value)
            {
                var taskAddObject = isAsync ? Task.FromResult(objectContext.Add(ent)) : objectContext.AddAsync(ent);
                return ((Task<int>) taskAddObject).ContinueWith(
                    t => t.Result >= 0 ? AppServiceExecuteStatus.Success : AppServiceExecuteStatus.Fail,
                    TaskContinuationOptions.OnlyOnRanToCompletion);
            }

            return Task.FromResult(AppServiceExecuteStatus.DoNotContinue);
        }

        public virtual Task<AppServiceExecuteStatus> UpdateObject<TEntity>(TEntity ent = null, bool isAsync = false)
            where TEntity : class
        {
            if (PreOnUpdateHandler == null || OnUpdatingHandler == null)
            {
                throw new NullReferenceException("更新事件未注册");
            }

            var objectContext = GetObjectContextDynamical<TEntity>();
            var existObject = PreOnUpdateHandler?.Invoke();
            if (existObject != null)
            {
                OnUpdatingHandler?.Invoke(existObject, ent);
                var entDbProxy = (IDBProxy) objectContext;
                var taskUpdateObject = isAsync ? Task.FromResult(entDbProxy.Commit()) : entDbProxy.CommitAsync();
                return taskUpdateObject.ContinueWith(
                    t => t.Result >= 0 ? AppServiceExecuteStatus.Success : AppServiceExecuteStatus.Fail,
                    TaskContinuationOptions.OnlyOnRanToCompletion);
            }

            return Task.FromResult(AppServiceExecuteStatus.DoNotContinue);
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
