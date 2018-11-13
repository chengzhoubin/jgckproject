using JGCK.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace JGCK.Web.General.WebAPI
{
    public class JGCK_ApiController : ApiController
    {
        public JGCK_ApiController()
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
    }
}
