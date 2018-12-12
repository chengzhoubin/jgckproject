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


namespace JGCK.Modules.ProductModule
{
    public class ProductManager : AbstractConfigurationService
    {

        public Task<List<Product>> GetProductListAsync(
            Expression<Func<Product, bool>> search,
            AbstractUnitOfWork.OrderByExpression<Product>[] orderBy,
            int pageIndex)
        {
            var pager = new AbstractUnitOfWork.Pager { CurrentIndex = pageIndex };
            return productDbContext.GetObjectsAsync(
                search,
                pager,
                false,
                orderBy,
                p => p.ProductNO,
                p => p.ProductNO);
        }

        public Task<int> GetProductCount(Expression<Func<Product, bool>> search)
        {
            return productDbContext.Product.CountAsync(search);
        }

        public bool ProductIsExists(string name)
        {
            return productDbContext.Product.Any(p => p.Name == name && !p.IsDeleted);
        }

        public Product GetProduct(long productId)
        {
            return productDbContext.Product.FirstOrDefault(p => p.ID == productId && !p.IsDeleted);
        }

        public Product GetProduct(string productName)
        {
            return productDbContext.Product.FirstOrDefault(p => p.Name == productName && !p.IsDeleted);
        }

        public Task<List<Product>> GetAllProductListAsync(Expression<Func<Product, bool>> search)
        {
            return productDbContext.GetObjectsAsync(search,
                null,
                true,
                null,
                p => p.ID,
                p => p.ProductTypeInfo);
        }

        public List<ProductTypeInfo> GetProductTypeListByParentId(long productTypeId)
        {
            if (productTypeId > 0)
            {
                return productDbContext.ProductTypeInfo.Where(p => p.ParentId == productTypeId).ToList();
            }
            else
            {
                return productDbContext.ProductTypeInfo.Where(p => !p.ParentId.HasValue).ToList();
            }
        }
    }
}
