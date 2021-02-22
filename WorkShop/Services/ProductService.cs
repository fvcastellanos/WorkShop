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

        public Either<string, IEnumerable<ProductView>> GetProducts(int top = 25, int active = 1)
        {
            try
            {
                _logger.LogInformation("get top {0} products with active value {1}", top, active);
                var queryResult = from product in _dbContext.Products
                    where product.Active == active
                    select new ProductView()
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
                
                return queryResult
                    .Take(top)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("can't get product list - {0}", ex.Message);
                return string.Format("Can't get product list");
            }
        }

        // -------------------------------------------------------------------------------------

        private DateTime LastNDays(int days)
        {
            return DateTime.Today.AddDays(days * -1);
        }
    }
}