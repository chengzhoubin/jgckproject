﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using JGCK.Framework;

namespace JGCK.Web.General.MVC
{
    public class GlobalDisposeFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
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
                ((List<object>) toDisposeObject).ForEach(item => ((IDBProxy)item).Dispose());
            });
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //throw new NotImplementedException();
        }
    }
}
