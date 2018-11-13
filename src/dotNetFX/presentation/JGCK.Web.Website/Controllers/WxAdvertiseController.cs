using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HSMY.BizLogic.Ops;
using HSMY.Web.General.WebAPI;
using HSMY_WxWeb.Models;

namespace HSMY_WxWeb.Controllers
{
    public class WxAdvertiseController : HSMY_ApiController
    {
        private AdService MAdService { get; set; }

        [HttpPost]
        public List<VM_Wx_Advertise> GetAdvertises(VM_Wx_AdFilter f)
        {
            var query = MAdService.GetAdsQueryable(f.CombineExpression())
                .OrderBy(m=>m.sort)
                .ThenByDescending(m => m.id)
                .Skip((f.CurrentIndex - 1) * f.PageSize)
                .Take(f.PageSize);
            return query.ToList().Select(m => new VM_Wx_Advertise
            {
                AdvID = m.id,
                AdvTitle = m.title,
                ImgUrl = m.img_url?.Replace("http://resource.bestdoctor1.com", "https://resource1.bestdoctor1.com"),
                RedirectUrl = m.link_url
            }).ToList();
        }
    }
}
