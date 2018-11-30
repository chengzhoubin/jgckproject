using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using JGCK.Framework;

namespace JGCK.Web.General
{
    public abstract class JGCK_MvcController : Controller
    {
        protected virtual string m_ModuleName => "";

        public JGCK_MvcController()
        {
            var propsInController = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            var appServiceProps = propsInController
                .Where(p => p.PropertyType.GetInterface(typeof(ITransistService).FullName) != null).ToList();
            if (appServiceProps?.Count == 0)
                return;
            appServiceProps.ForEach(p =>
            {
                var refObject = p.PropertyType.Assembly.CreateInstance(p.PropertyType.FullName);
                p.SetValue(this, refObject);
            });

            var usedAppServices = appServiceProps
                .Select(p => string.Format(HostVer.IDBProxy_Slot_Format, p.PropertyType.Name)).ToList();
            CallContext.LogicalSetData(HostVer.ReferenceService_VerName, usedAppServices);
            //CallContext.SetData(HostVer.ReferenceService_VerName, usedAppServices);
        }

        protected bool IsGetMethod => Request.HttpMethod == "GET";
    }
}
