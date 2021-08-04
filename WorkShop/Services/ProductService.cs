using System;
using System.Linq;
using System.Collections.Generic;
using LanguageExt;
using Microsoft.Extensions.Logging;
using WorkShop.Domain;
using WorkShop.Clients;
using WorkShop.Providers;
using Microsoft.AspNetCore.Http;
using WorkShop.Model;

namespace WorkShop.Services
{
    public class ProductService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly ProductClient _productClient;

        private readonly WorkShopContext _dbContext;

        public ProductService(ILogger<ProductService> logger, 
                              WorkShopContext workShopContext,
                              ProductClient productClient,
                              IHttpContextAccessor httpContextAccessor,
                              TokenProvider tokenProvider): base(httpContextAccessor, tokenProvider)
        {
            _logger = logger;
            _dbContext = workShopContext;
            _productClient = productClient;
        }

        public Either<string, IEnumerable<ProductView>> GetProducts(int top = 25, string code = "", string name = "", int active = 1)
        {
            try
            {
                _logger.LogInformation("get top {0} products with active value {1}", top, active);

                return _dbContext.Products.Where(product => product.Active.Equals(active) && product.Code.Contains(code)
                    && product.Name.Contains(name))
                    .Take(top)
                    .Select(ToView)
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
                var productHolder = FindById(productView.Code);

                if (productHolder.IsSome) {

                    return $"Code {productView.Code} already exists";
                }

                var product = new Product()
                {
                        Code = productView.Code,
                        Name = productView.Name,
                        Description = productView.Description,
                        MinimalAmount = productView.MinimalAmount,
                        Created = DateTime.Now,
                        Tenant = DefaultTenant,
                        Active = 1
                };

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                productView.Id = product.Id.ToString();

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
                var product = _dbContext.Products.Find(Guid.Parse(productView.Id));

                if (product == null)
                {
                    return $"Product with id: {productView.Id} not found";
                }

                product.Code = productView.Code;
                product.Name = productView.Name;
                product.Description = productView.Description;
                product.MinimalAmount = productView.MinimalAmount;
                product.Active = productView.Active;
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

        public Option<ProductView> FindById(string id)
        {
            try
            {
                return _dbContext.Products.Where(product => product.Id.Equals(Guid.Parse(id)))
                    .Map(ToView)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("can't find product using id: {0} - {1} ", id, ex.Message);
                return null;
            }
        }

        // -------------------------------------------------------------------------------------

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
                Active = productView.Active
            };
        }

        private static ProductView ToView(Product product)
        {
            return new ProductView
            {
                Id = product.Id.ToString(),
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                MinimalAmount = product.MinimalAmount,
                SalePrice = product.SalePrice,
                Active = product.Active
            };
        }
    }
}