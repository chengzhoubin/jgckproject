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
    public class JGCK_MvcController : Controller
    {
        public JGCK_MvcController()
        {
            var propsInController = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            var appServiceProps = propsInController
                .Where(p => p.PropertyType.GetInterface(typeof(ITransistService).FullName) != null).ToList();
            if (appServiceProps == null || appServiceProps.Count == 0)
                return;
            appServiceProps.ForEach(p =>
            {
                var refObject = p.PropertyType.Assembly.CreateInstance(p.PropertyType.FullName);
                p.SetValue(this, refObject);
            });
            CallContext.SetData(HostVer.ReferenceService_VerName,
                appServiceProps.Select(p => string.Format(HostVer.IDBProxy_Slot_Format, p.PropertyType.Name)).ToList());
        }

        protected bool IsGetMethod => Request.HttpMethod == "GET";
    }
}
