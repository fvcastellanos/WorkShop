using System;
using System.Linq;
using System.Collections.Generic;
using LanguageExt;
using Microsoft.Extensions.Logging;
using WorkShop.Model;
using WorkShop.Domain;

namespace WorkShop.Services
{
    public class ProductService
    {
        private readonly ILogger _logger;
        private readonly WorkShopContext _dbContext;

        public ProductService(ILogger<ProductService> logger, WorkShopContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Either<string, IEnumerable<ProductView>> GetProducts(int top = 25, string code = "", string name = "", int active = 1)
        {
            try
            {
                _logger.LogInformation("get top {0} products with active value {1}", top, active);

                return _dbContext.Products
                    .Where(product => product.Active == active
                            && product.Code.Contains(code)
                            && product.Name.Contains(name))
                    .Select(ToProductView)
                    .Take(top)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("can't get product list - {0}", ex.Message);
                return "Can't get product list";
            }
        }

        public Either<string, ProductView> AddProduct(ProductView productView)
        {
            try
            {
                var productHolder = FindByCode(productView.Code);

                if (productHolder.IsSome) {

                    return $"Code {productView.Code} already exists";
                }

                var id = Guid.NewGuid().ToString();
                var product = new Product()
                {
                        Id = id,
                        Code = productView.Code,
                        Name = productView.Name,
                        Description = productView.Description,
                        MinimalAmount = productView.MinimalAmount,
                };

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return productView;
            }
            catch (Exception ex)
            {
                _logger.LogError("can't create new product with name: {0} - {1}", productView.Name, ex.Message);
                return $"Can't create product with name: {productView.Name}";
            }
        }

        public Option<ProductView> FindByCode(string code)
        {
            try
            {
                var product = _dbContext.Products
                    .Where(product => product.Code.Equals(code))
                    .Select(ToProductView)
                    .FirstOrDefault();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError("can't find product using code: {0} - {1} ", code, ex.Message);
                return null;
            }
        }

        // -------------------------------------------------------------------------------------

        private static ProductView ToProductView(Product product)
        {
            return new ProductView()
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                MinimalAmount = product.MinimalAmount,
                SalePrice = product.SalePrice,
                Created = product.Created,
                Updated = product.Updated,
                Active = product.Active
            };
        }
    }
}