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
        private const string DefaultTenant = "default";
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
                    .Where(product => product.Active.Equals(active)
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

        public Either<string, ProductView> Add(ProductView productView)
        {
            try
            {
                var productHolder = FindByCode(productView.Code);

                if (productHolder.IsSome) {

                    return $"Code {productView.Code} already exists";
                }

                var product = new Product()
                {
                        Code = productView.Code,
                        Name = productView.Name,
                        Description = productView.Description,
                        MinimalAmount = productView.MinimalAmount,
                        Tenant = DefaultTenant
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

        public Either<string, ProductView> Update(ProductView productView)
        {
            try
            {
                var id = Guid.Parse(productView.Id);
                var product = _dbContext.Products.Find(id);

                if (product == null)
                {
                    return $"Product with id: {productView.Id} not found";
                }

                product.Name = productView.Name;
                product.Description = productView.Description;
                product.Code = productView.Code;
                product.MinimalAmount = productView.MinimalAmount;
                // product.Active = productView.Active
                product.Updated = DateTime.Now;

                _dbContext.Products.Update(product);
                _dbContext.SaveChanges();

                return productView;
            }
            catch (Exception ex)
            {
                _logger.LogError("can't update product with id: {0} - {1}", productView.Id, ex.Message);
                return $"Can't update product with id: {productView.Id}";
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
                Id = product.Id.ToString(),
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                MinimalAmount = product.MinimalAmount,
                SalePrice = product.SalePrice,
                Created = product.Created,
                Updated = product.Updated,
                // Active = product.Active
            };
        }

        private static Product ToProduct(ProductView productView)
        {
            return new Product()
            {
                Id = Guid.Parse(productView.Id),
                Code = productView.Code,
                Name = productView.Name,
                Description = productView.Description,
                MinimalAmount = productView.MinimalAmount,
                SalePrice = productView.SalePrice,
                Created = productView.Created,
                Updated = productView.Updated,
                // Active = productView.Active
            };
        }
    }
}