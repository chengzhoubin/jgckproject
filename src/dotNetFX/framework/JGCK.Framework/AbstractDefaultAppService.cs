using System;
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
    }
}
