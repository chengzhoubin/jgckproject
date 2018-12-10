using JGCK.Framework.EF;
using JGCK.Modules.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JGCK.Respority.ProductWork;


namespace JGCK.Modules.Product
{
    public class ProductManager : AbstractConfigurationService
    {
        public Task<List<JGCK.Respority.ProductWork.Product>> GetProductListAsync(
            Expression<Func<JGCK.Respority.ProductWork.Product, bool>> search,
            AbstractUnitOfWork.OrderByExpression<JGCK.Respority.ProductWork.Product>[] orderBy,
            int pageIndex)
        {
            var pager = new AbstractUnitOfWork.Pager { CurrentIndex = pageIndex };
            return basicDbContext.GetObjectsAsync(
                search,
                pager,
                false,
                orderBy,
                p => p.ProductNO,
                p => p.ProductNO);
        }

        public Task<int> GetProductCount(Expression<Func<JGCK.Respority.ProductWork.Product, bool>> search)
        {
            //return basicDbContext.Product.CountAsync(search);
            return null;
        }
    }
}
