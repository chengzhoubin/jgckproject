using JGCK.Respority.ProductWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JGCK.Web.Admin.Models.Mapper
{
    public static class VmProductMapper
    {
        static VmProductMapper()
        {
            if (!ExpressMapper.Mapper.MapExists(typeof(Product), typeof(Product)))
                ExpressMapper.Mapper.Register<Product, Product>();
                    //.Ignore<Role>(p => p.Role);
        }

        public static Product MapTo(this Product existProduct, Product targetProduct)
        {
            return ExpressMapper.Mapper.Map(existProduct, targetProduct);
        }
    }
}