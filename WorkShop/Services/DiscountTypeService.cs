
using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.Extensions.Logging;
using WorkShop.Domain;
using WorkShop.Model;
using WorkShop.Repositories;

namespace WorkShop.Services
{
    public class DiscountTypeService : ServiceBase
    {
        private readonly ILogger _logger;

        private readonly DiscountTypeRepository _discountTypeRepository;

        public DiscountTypeService(ILogger<DiscountTypeService> logger, 
                                   DiscountTypeRepository discountTypeRepository)
        {
            _logger = logger;
            _discountTypeRepository = discountTypeRepository;
        }        

        public Either<string, IEnumerable<DiscountTypeView>> GetDiscountTypes(int topRows, string name, int active)
        {
            try
            {
                _logger.LogInformation($"get top {topRows} discount types");

                return _discountTypeRepository.FindDiscountTypes(topRows, name, active)
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
                var discountTypeHolder = _discountTypeRepository.FindByName(discountTypeView.Name);

                if (discountTypeHolder.IsSome)
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

                var storedEntity = _discountTypeRepository.Add(discountType);

                return ToView(storedEntity);
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
                var discountTypeHolder = _discountTypeRepository.FindById(view.Id);

                if (discountTypeHolder.IsNone)
                {
                    return $"Discount Type not found {view.Name}";
                }

                var discountType = discountTypeHolder.FirstOrDefault();
                discountType.Name = view.Name;
                discountType.Description = view.Description;
                discountType.Active = view.Active;
                discountType.Updated = DateTime.Now;

                var storedEntity = _discountTypeRepository.Update(discountType);

                return ToView(storedEntity);
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
                return _discountTypeRepository.FindById(id)
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
