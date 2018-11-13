using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using JGCK.Framework;

namespace JGCK.Web.General.WebAPI
{
    public class ApiGlobalDisposeFilterAttribute : ActionFilterAttribute, IDisposable
    {
        public void Dispose()
        {
            var allToDisposeObjectName = CallContext.GetData(HostVer.ReferenceService_VerName);
            if (allToDisposeObjectName == null)
                return;
            var allToDisposeObjectNameList = allToDisposeObjectName as List<string>;
            if (allToDisposeObjectNameList == null || allToDisposeObjectNameList.Count == 0)
                return;
            allToDisposeObjectNameList.ForEach(slotName =>
            {
                var toDisposeObject = CallContext.GetData(slotName);
                if (toDisposeObject == null)
                    return;
                ((List<object>) toDisposeObject).ForEach(item =>
                {
                    ((IDBProxy) item).Dispose();
                });
            });
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            this.Dispose();
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
