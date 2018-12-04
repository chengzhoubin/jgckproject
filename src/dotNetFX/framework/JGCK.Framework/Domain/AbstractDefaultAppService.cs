﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JGCK.Framework
{
    public class AbstractDefaultAppService : IAppService
    {
        private IList<IDBProxy> proxyContrainer;
        public Func<object, bool> PreLogicDeleteHandler { get; set; }

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

        protected virtual Task<int> LogicObjectDelete<TObjectContext, TEntity, T>(T pkId, bool isAsync = false)
            where TEntity : class
            where TObjectContext : IDBProxy
        {
            var propsInTypeInfos = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            var objectContextPropertyInfo =
                propsInTypeInfos.FirstOrDefault(p => p.PropertyType == typeof(TObjectContext));
            var objectContext = (dynamic) objectContextPropertyInfo?.GetValue(this);
            if (objectContext == null)
            {
                throw new NullReferenceException();
            }

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
            var entDbProxy = (IDBProxy) entObject;
            return isAsync
                ? Task.FromResult(entDbProxy.Commit())
                : entDbProxy.CommitAsync();
        }
    }
}
