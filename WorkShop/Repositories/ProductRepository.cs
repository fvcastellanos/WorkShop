using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using WorkShop.Model;

namespace WorkShop.Repositories
{
    public class ProductRepository
    {
        private readonly WorkShopContext _dbContext;

        public ProductRepository(WorkShopContext workShopContext)
        {
            _dbContext = workShopContext;
        }

        public IEnumerable<Product> FindProducts(int top, string code, string name, int active)
        {
            return _dbContext.Products.Where(product => product.Active.Equals(active) 
                    && product.Code.Contains(code)
                    && product.Name.Contains(name))
                .Take(top)
                .ToList();            
        }

        public Option<Product> FindByCode(string code)
        {
            return _dbContext.Products.FirstOrDefault(product => 
                    product.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));
        }

        public Option<Product> FindById(string id)
        {
            return _dbContext.Products.FirstOrDefault(product => product.Id.Equals(Guid.Parse(id)));
        }

        public IEnumerable<Product> GetActiveProducts()
        {
            return _dbContext.Products.Where(product => product.Active.Equals(1))
                .ToList();
        }

        public Product Add(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return product;
        }

        public Product Update(Product product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();

            return product;
        }

    }
}