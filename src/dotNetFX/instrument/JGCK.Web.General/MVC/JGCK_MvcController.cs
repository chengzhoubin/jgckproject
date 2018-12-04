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
using JGCK.Framework.EF;
using JGCK.Util.Enums;
using JGCK.Web.General.VO;

namespace JGCK.Web.General
{
    public abstract class JGCK_MvcController : Controller
    {
        //protected virtual string m_ModuleName => "";

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

        protected virtual AbstractUnitOfWork.OrderByExpression<T>[] UserSortBy<T, TSortValue>(string modulename)
            where T : class
            where TSortValue : ISortValue
        {
            var keyOfSort = modulename;//$"{modulename}_sort_keys";
            var jsonSortValue = CookieHelper.GetValue<List<TSortValue>>(keyOfSort, false);
            var orderByExps = new List<AbstractUnitOfWork.OrderByExpression<T>>();
            jsonSortValue?.ForEach(v =>
            {
                var item = new AbstractUnitOfWork.OrderByExpression<T>();
                item.OrderByExpressionMember = v.SortProperty;
                item.SortBy = v.SortDirect;
                orderByExps.Add(item);
            });

            if (jsonSortValue == null || orderByExps.Count == 0)
            {
                orderByExps.Add(new AbstractUnitOfWork.OrderByExpression<T>
                {
                    OrderByExpressionMember = "ID",
                    SortBy = AscOrDesc.Desc
                });
            }

            return orderByExps.ToArray();
        }
    }
}
