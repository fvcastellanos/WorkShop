
using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WorkShop.Clients;
// using WorkShop.Clients.Domain;
using WorkShop.Domain;
using WorkShop.Model;
using WorkShop.Providers;

namespace WorkShop.Services
{
    public class DiscountTypeService : ServiceBase
    {
        private readonly ILogger _logger;

        private readonly WorkShopContext _dbContext;

        private readonly DiscountTypeClient _discountTypeClient;

        public DiscountTypeService(ILogger<DiscountTypeService> logger, 
                                   WorkShopContext workShopContext,
                                   DiscountTypeClient discountTypeClient,
                                   TokenProvider tokenProvider,
                                   IHttpContextAccessor httpContextAccessor): base(httpContextAccessor, tokenProvider)
        {
            _logger = logger;
            _dbContext = workShopContext;
            _discountTypeClient = discountTypeClient;
        }        

        public Either<string, IEnumerable<DiscountTypeView>> GetDiscountTypes(int topRows, string name, int active)
        {
            try
            {
                _logger.LogInformation($"get top {topRows} discount types");

                return _dbContext.DiscountTypes.Where(discountType => discountType.Active.Equals(active) && 
                    discountType.Name.Contains(name))
                    .Take(topRows)
                    .Select(ToView)
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
                var existingDiscountType = _dbContext.DiscountTypes.Where(discountType => discountType.Name.Equals(discountType.Name))
                    .FirstOrDefault();

                if (existingDiscountType != null)
                {
                    return $"Discount Type: {discountTypeView.Name} already exists";
                }

                var discountType = new DiscountType
                {
                    Name = discountTypeView.Name,
                    Description = discountTypeView.Description,
                    Created = DateTime.Now,
                    Tenant = DefaultTenant,
                    Active = 1
                };

                _dbContext.DiscountTypes.Add(discountType);
                _dbContext.SaveChanges();

                return discountTypeView;
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't add discount type: {discountTypeView.Name} - ", ex.Message);
                return $"Can't add discount type: {discountTypeView.Name}";
            }
        }

        public Either<string, DiscountTypeView>Update(DiscountTypeView view)
        {
            try
            {
                var discountType = _dbContext.DiscountTypes.Find(Guid.Parse(view.Id));

                if (discountType == null)
                {
                    return $"Discount Type not found {view.Name}";
                }

                discountType.Name = view.Name;
                discountType.Description = view.Description;
                discountType.Active = view.Active;
                discountType.Updated = DateTime.Now;

                _dbContext.Update(discountType);
                _dbContext.SaveChanges();

                return view;
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't update discount type: {view.Name} - ", ex.Message);
                return $"Can't update discount type: {view.Name}";
            }
        }

        public Option<DiscountTypeView> FindById(string id)
        {
            try
            {
                return _dbContext.DiscountTypes.Where(discountType => discountType.Id.Equals(Guid.Parse(id)))
                    .Map(ToView)
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