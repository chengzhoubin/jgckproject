using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
                //context.Response.Write("test12");
            };
            //throw new NotImplementedException();
        }
    }
}
