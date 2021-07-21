
using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WorkShop.Clients;
using WorkShop.Clients.Domain;
using WorkShop.Domain;
using WorkShop.Providers;

namespace WorkShop.Services
{
    public class DiscountTypeService : ServiceBase
    {
        private readonly ILogger _logger;

        private readonly DiscountTypeClient _discountTypeClient;

        public DiscountTypeService(ILogger<DiscountTypeService> logger, 
                                   DiscountTypeClient discountTypeClient,
                                   TokenProvider tokenProvider,
                                   IHttpContextAccessor httpContextAccessor): base(httpContextAccessor, tokenProvider)
        {
            _logger = logger;
            _discountTypeClient = discountTypeClient;
        }        

        public Either<string, IEnumerable<DiscountTypeView>> GetDiscountTypes(int topRows, string name, int active)
        {
            try
            {
                _logger.LogInformation($"get top {topRows} discount types");

                return _discountTypeClient.Find(GetStrapiToken(), topRows, name, active)
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
                var discountTypeHolder = _discountTypeClient.FindByName(GetStrapiToken(), discountTypeView.Name);

                if (discountTypeHolder.IsSome)
                {
                    return $"Discount Type: {discountTypeView.Name} already exists";
                }

                var discountType = new DiscountType
                {
                    Name = discountTypeView.Name,
                    Description = discountTypeView.Description,
                    Active = true
                };

                _discountTypeClient.Add(GetStrapiToken(), discountType);

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
                var discountTypeHolder = FindById(view.Id);
                var error = "";

                discountTypeHolder.Match(some => {

                    var discountType = new DiscountType
                    {
                        Id = long.Parse(view.Id),
                        Name = view.Name,
                        Description = view.Description,
                        Active = true
                    };

                    _discountTypeClient.Update(GetStrapiToken(), discountType);

                }, () => error = $"Discount Type not found {view.Name}");

                if (!string.IsNullOrEmpty(error))
                {
                    return error;
                }

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
                return _discountTypeClient.FindById(GetStrapiToken(), id)
                    .Map(ToView);
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
                Active = discountType.Active ? 1 : 0
            };
        }
    }
}