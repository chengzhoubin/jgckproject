using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JGCK.Web.General;
using JGCK.Web.Admin.Models;
using Newtonsoft.Json;
using JGCK.Respority.ProductWork;
using JGCK.Web.General.Helper;
using JGCK.Modules.Product;
using System.Threading.Tasks;

namespace JGCK.Web.Admin.Controllers
{
    public class ProductController : JGCK_MvcController
    {
        // GET: Product..
        public ActionResult Index()
        {
            return View();
        }

        private ProductManager m_ProductService { get; set; }

        [HttpGet]
        public async Task<ActionResult> ProductList(string filter, int? p)
        {
            var productIndex = new VmProductfIndex() { Filter = filter?.Trim() };
            var pageIndex = p.HasValue ? p.Value : 1;
            var searchExp = productIndex.CombineExpression();
            var entList =
                await m_ProductService.GetProductListAsync(
                    searchExp,
                    UserSortBy<Product, JsonSortValue>(ConfigHelper.KeyModuleProductSort),
                    pageIndex);
            productIndex.TotalRecordCount = await m_ProductService.GetProductCount(searchExp);
            productIndex.ViewObjects = entList.Select(item => new VmProduct()
            {
                NagigatedDomainObject = item,
                ResetSettingHandler = () =>
                {
                    JsonConvert.DefaultSettings = () =>
                    {
                        var js = new JsonSerializerSettings();
                        js.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        return js;
                    };
                }
            }).ToList();
            productIndex.CurrentIndex = pageIndex;
            return View(productIndex);
        }

        //[HttpPost]
        //public async Task<JsonResult> AddStaff(VmStaff staff)
        //{
        //    var ret = new VM_JsonOnlyResult();
        //    var modelState = (new VmStaffValidator()).Validate(staff);
        //    if (!modelState.IsValid)
        //    {
        //        ret.Value = -1001;
        //        ret.Err = string.Join(",", modelState.Errors.Select(m => m.ErrorMessage));
        //        return await Task.FromResult(Json(ret));
        //    }

        //    m_UserManagerService.PreOnAddHandler =
        //        () => !m_UserManagerService.UserIsExists(staff.NagigatedDomainObject.Name);
        //    var added = await m_UserManagerService.AddObject(staff.NagigatedDomainObject, true);
        //    if (added == AppServiceExecuteStatus.Success)
        //    {
        //        ret.Value = staff.NagigatedDomainObject.ID;
        //        ret.Result = true;
        //        return Json(ret);
        //    }

        //    ret.Err = added.ToDescription();
        //    return Json(ret);
        //}

        //[HttpPost]
        //public async Task<JsonResult> DeleteStaff(long staffId)
        //{
        //    var ret = new VM_JsonOnlyResult();
        //    var deleted = await m_UserManagerService.LogicObjectDelete<Person, long>(staffId, true);
        //    if (deleted == AppServiceExecuteStatus.Success)
        //    {
        //        ret.Value = staffId;
        //        ret.Result = true;
        //        return Json(ret);
        //    }

        //    ret.Err = deleted.ToDescription();
        //    return Json(ret);
        //}

        //[HttpPost]
        //public async Task<JsonResult> UpdateStaff(VmStaff staff)
        //{
        //    var ret = new VM_JsonOnlyResult();
        //    var modelState = (new VmStaffValidator()).Validate(staff);
        //    if (!modelState.IsValid)
        //    {
        //        ret.Value = -1001;
        //        ret.Err = string.Join(",", modelState.Errors.Select(m => m.ErrorMessage));
        //        return await Task.FromResult(Json(ret));
        //    }

        //    m_UserManagerService.PreOnUpdateHandler =
        //        () => m_UserManagerService.GetUser(staff.NagigatedDomainObject.ID);
        //    m_UserManagerService.OnUpdatingHandler = (existOject, newObject) =>
        //    {
        //        ((Person)newObject).MapTo((Person)existOject);
        //    };
        //    var updatedRet = await m_UserManagerService.UpdateObject(staff.NagigatedDomainObject, true);
        //    if (updatedRet == AppServiceExecuteStatus.Success)
        //    {
        //        ret.Value = staff.NagigatedDomainObject.ID;
        //        ret.Result = true;
        //        return Json(ret);
        //    }

        //    ret.Err = updatedRet.ToDescription();
        //    return Json(ret);
        //}
    }
}