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
using JGCK.Modules.ProductModule;
using System.Threading.Tasks;
using HSMY_AdminWeb.Models;
using JGCK.Web.Admin.Models.Validator;
using JGCK.Framework;
using JGCK.Util;
using JGCK.Web.Admin.Models.Mapper;

namespace JGCK.Web.Admin.Controllers
{
    public class ProductController : JGCK_MvcController
    {
        // GET: Product..
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductType()
        {
            return View();
        }

        private ProductManager m_ProductService { get; set; }

        [HttpGet]
        public async Task<ActionResult> AllProductList()
        {
            var productIndex = new VmProductIndex() { };
            var searchExp = productIndex.CombineExpression();
            var entList =
                await m_ProductService.GetAllProductListAsync(
                    searchExp
                    );
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
            return View(productIndex);
        }

        [HttpPost]
        public async Task<JsonResult> AddProduct(VmProduct product)
        {
            var ret = new VM_JsonOnlyResult();
            var modelState = (new VmProductValidator()).Validate(product);
            if (!modelState.IsValid)
            {
                ret.Value = -1001;
                ret.Err = string.Join(",", modelState.Errors.Select(m => m.ErrorMessage));
                return await Task.FromResult(Json(ret));
            }

            m_ProductService.PreOnAddHandler =
                () => !m_ProductService.ProductIsExists(product.NagigatedDomainObject.Name);
            var added = await m_ProductService.AddObject(product.NagigatedDomainObject, true);
            if (added == AppServiceExecuteStatus.Success)
            {
                ret.Value = product.NagigatedDomainObject.ID;
                ret.Result = true;
                return Json(ret);
            }

            ret.Err = added.ToDescription();
            return Json(ret);
        }

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

        [HttpPost]
        public async Task<JsonResult> UpdateProduct(VmProduct product)
        {
            var ret = new VM_JsonOnlyResult();
            var modelState = (new VmProductValidator()).Validate(product);
            if (!modelState.IsValid)
            {
                ;
                ret.Value = -1001;
                ret.Err = string.Join(",", modelState.Errors.Select(m => m.ErrorMessage));
                return await Task.FromResult(Json(ret));
            }

            m_ProductService.PreOnUpdateHandler =
                () =>
                {
                    var selfProduct = m_ProductService.GetProduct(product.NagigatedDomainObject.ID);
                    if (selfProduct == null)
                        return null;
                    var otherUser = m_ProductService.GetProduct(product.NagigatedDomainObject.Name);
                    if (otherUser == null || otherUser.ID == selfProduct.ID)
                        return selfProduct;
                    return null;
                };
            m_ProductService.OnUpdatingHandler = (existOject, newObject) =>
            {
                ((Product)newObject).MapTo((Product)existOject);
            };
            var updatedRet = await m_ProductService.UpdateObject(product.NagigatedDomainObject, true);
            if (updatedRet == AppServiceExecuteStatus.Success)
            {
                ret.Value = product.NagigatedDomainObject.ID;
                ret.Result = true;
                return Json(ret);
            }

            ret.Err = updatedRet.ToDescription();
            return Json(ret);
        }

        [HttpGet]
        public async Task<JsonResult> GetProductTypeListByParentId(long parentId,string parentName)
        {
            var ret = new VM_JsonOnlyResult();
            VmProductTree productTree = new VmProductTree();
            productTree.Id = parentId;
            productTree.Name = parentName;
            IList<ProductTypeInfo> productTypes = m_ProductService.GetProductTypeListByParentId(parentId);
            productTree.children = new List<ProductTreeChildren>();
            if (productTypes!=null&& productTypes.Count>0)
            {
                foreach (var pt in productTypes)
                {
                    ProductTreeChildren pc = new ProductTreeChildren()
                    {
                        Name = pt.Name,
                        Id = pt.ID
                    };
                    productTree.children.Add(pc);
                }
            }
            ret.Value = productTree;
            ret.Result = true;
            return Json(ret);
        }
    }
}