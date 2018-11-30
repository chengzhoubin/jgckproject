using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JGCK.Framework;

namespace JGCK.Web.General.HttpModules
{
    internal class DynamicBindServiceModules : IHttpModule
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += (o, e) =>
            {
                //var allToDisposeObjectName = CallContext.GetData(HostVer.ReferenceService_VerName);
                var allToDisposeObjectName = CallContext.LogicalGetData(HostVer.ReferenceService_VerName);
                if (allToDisposeObjectName == null)
                    return;
                var allToDisposeObjectNameList = allToDisposeObjectName as List<string>;
                if (allToDisposeObjectNameList?.Count == 0)
                    return;
                allToDisposeObjectNameList?.ForEach(slotName =>
                {
                    var toDisposeObject = CallContext.GetData(slotName);
                    if (toDisposeObject == null)
                        return;
                    ((List<object>)toDisposeObject).ForEach(item => ((IDBProxy)item).Dispose());
                });
                CallContext.FreeNamedDataSlot(HostVer.ReferenceService_VerName);
            };
        }
    }
}
