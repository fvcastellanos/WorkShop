
using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.Extensions.Logging;
using WorkShop.Domain;
using WorkShop.Model;

namespace WorkShop.Services
{
    public class DiscountTypeService : ServiceBase
    {
        private readonly ILogger _logger;

        private readonly WorkShopContext _dbContext;

        public DiscountTypeService(ILogger<DiscountTypeService> logger, WorkShopContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }        

        public Either<string, IEnumerable<DiscountTypeView>> GetDiscountTypes(int topRows, string name, int active)
        {
            try
            {
                _logger.LogInformation($"get top {topRows} discount types");
                
                return _dbContext.DiscountTypes
                    .Where(d => d.Active == active
                        && d.Name.Contains(name))
                    .Select(ToView)
                    .Take(topRows)
                    .ToList();                    
            }
            catch (Exception ex)
            {
                _logger.LogError("can't get discount types - ", ex);
                return "Can't get discount types";
            }
        }

        public Either<string, DiscountTypeView> Add(DiscountTypeView discountTypeView)
        {
            try
            {
                var existing = _dbContext.DiscountTypes
                    .Where(dt => dt.Name.ToLower().Equals(discountTypeView.Name.ToLower()))
                    .FirstOrDefault();

                if (existing != null)
                {
                    return $"Discount Type: {discountTypeView.Name} already exists";
                }

                var discountType = new DiscountType()
                {
                    Name = discountTypeView.Name,
                    Description = discountTypeView.Description,
                    Tenant = DefaultTenant
                };

                _dbContext.DiscountTypes.Add(discountType);
                _dbContext.SaveChanges();

                discountTypeView.Id = discountType.Id.ToString();

                return discountTypeView;
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't add discount type: {discountTypeView.Name} - ", ex.Message);
                return $"Can't add discount type: {discountTypeView.Name}";
            }
        }

        public Option<DiscountTypeView> FindById(string id)
        {
            try
            {
                return _dbContext.DiscountTypes
                    .Where(dt => dt.Id.Equals(Guid.Parse(id)))
                    .Select(ToView)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't find discount type with id: {id} - ", ex.Message);
                return null;
            }
        }

        // ------------------------------------------------------------------------

        private DiscountTypeView ToView(DiscountType discountType)
        {
            return new DiscountTypeView()
            {
                Id = discountType.Id.ToString(),
                Name = discountType.Name,
                Description = discountType.Description,
                Active = discountType.Active
            };
        }
    }
}