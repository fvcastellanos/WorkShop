using System;
using System.Linq;
using System.Collections.Generic;
using LanguageExt;
using Microsoft.Extensions.Logging;
using WorkShop.Domain;
using WorkShop.Clients;
using WorkShop.Providers;
using Microsoft.AspNetCore.Http;
using WorkShop.Clients.Domain;

namespace WorkShop.Services
{
    public class ProductService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly ProductClient _productClient;

        public ProductService(ILogger<ProductService> logger, 
                              ProductClient productClient,
                              IHttpContextAccessor httpContextAccessor,
                              TokenProvider tokenProvider): base(httpContextAccessor, tokenProvider)
        {
            _logger = logger;
            _productClient = productClient;
        }

        public Either<string, IEnumerable<ProductView>> GetProducts(int top = 25, string code = "", string name = "", int active = 1)
        {
            try
            {
                _logger.LogInformation("get top {0} products with active value {1}", top, active);

                return _productClient.Find(GetStrapiToken(), top, code, name, active)
                    .Select(ToProductView)
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
                        Active = true
                };

                _productClient.Add(GetStrapiToken(), product);

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
                
                var productHolder = FindById(productView.Id);
                var error = "";                

                productHolder.Match(some => {
                    
                    var product = ToProduct(productView);

                    _productClient.Update(GetStrapiToken(), product);

                }, () => error = $"Product with id: {productView.Id} not found");

                if (!String.IsNullOrEmpty(error))
                {
                    return error;
                }

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
                return _productClient.FindById(GetStrapiToken(), id)
                    .Map(ToProductView);
            }
            catch (Exception ex)
            {
                _logger.LogError("can't find product using id: {0} - {1} ", id, ex.Message);
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
                Active = product.Active ? 1 : 0
            };
        }

        private static Product ToProduct(ProductView productView)
        {
            return new Product()
            {
                Id = long.Parse(productView.Id),
                Code = productView.Code,
                Name = productView.Name,
                Description = productView.Description,
                MinimalAmount = productView.MinimalAmount,
                SalePrice = productView.SalePrice,
                Active = productView.Active.Equals(1)
            };
        }
    }
}