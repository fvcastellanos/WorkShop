using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WorkShop.Clients;
using WorkShop.Domain;
using WorkShop.Model;
using WorkShop.Providers;

namespace WorkShop.Services
{
    public class ProviderService: ServiceBase
    {
        private readonly ILogger _logger;
        private readonly TokenProvider _tokenProvider;
        private readonly ProviderClient _providerClient;

        private WorkShopContext _dbContext;

        public ProviderService(ILogger<ProviderService> logger, 
                               WorkShopContext workShopContext,
                               ProviderClient providerClient,
                               TokenProvider tokenProvider,
                               IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, tokenProvider)
        {
            _logger = logger;
            _dbContext = workShopContext;
            _tokenProvider = tokenProvider;
            _providerClient = providerClient;
        }

        public Either<string, IEnumerable<ProviderView>> GetProviders(int topRows, string code, string name, int active)
        {
            try
            {
                _logger.LogInformation($"get top: {topRows} providers");

                return _dbContext.Providers.Where(provider => provider.Active.Equals(active) && provider.Code.Contains(code) 
                    && provider.Name.Contains(name))
                    .Take(topRows)
                    .Select(ToView)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("can't get providers - ", ex.Message);
                return "Can't get providers";
            }
        }

        public Either<string, ProviderView> Add(ProviderView view)
        {
            try
            {
                var holder = FindByCode(view.Code);

                if (holder.IsSome)
                {
                    return $"Provider code {view.Code} already exists";
                }

                var provider = new Provider
                {
                    Code = view.Code,
                    Name = view.Name,
                    Contact = view.Contact,
                    TaxId = view.TaxId,
                    Description = view.Description,
                    Created = DateTime.Now,
                    Tenant = DefaultTenant,
                    Active = 1
                };

                _dbContext.Providers.Add(provider);
                _dbContext.SaveChanges();

                view.Id = provider.Id.ToString();

                return view;
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't add provider with name: {view.Name} - ", ex.Message);
                return $"Can't add provider with name: {view.Name}";
            }
        }

        public Either<string, ProviderView> Update(ProviderView view)
        {
            try
            {
                var provider = _dbContext.Providers.Find(Guid.Parse(view.Id));

                if (provider == null)
                {
                    return $"Provider: {view.Name} not found";
                }

                provider.Code = view.Code;
                provider.Name = view.Name;
                provider.Contact = view.Contact;
                provider.Description = view.Description;
                provider.TaxId = view.TaxId;
                provider.Active = view.Active;
                provider.Updated = DateTime.Now;

                _dbContext.Update(provider);
                _dbContext.SaveChanges();

                return view;
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't update provider: {view.Name} - ", ex.Message);
                return $"Can't update provider: {view.Name}";
            }
        }

        public Option<ProviderView> FindByCode(string code)
        {
            try
            {
                _logger.LogInformation($"looking for provider with code: {code}");

                return _dbContext.Providers.Where(provider => provider.Code.Equals(Guid.Parse(code)))
                    .Map(ToView)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't find provider with id {code} - {ex.Message}");
                return null;
            }
        }

        public Option<ProviderView> FindById(string id)
        {
            try
            {
                _logger.LogInformation($"looking for provider with id: {id}");

                return _dbContext.Providers.Where(provider => provider.Id.Equals(Guid.Parse(id)))
                    .Map(ToView)
                    .FirstOrDefault();                    
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't find provider with id {id} - {ex.Message}");
                return null;
            }
        }

        // ---------------------------------------------------------------------------

       private ProviderView ToView(Provider provider)
        {
            return new ProviderView()
            {
                Id = provider.Id.ToString(),
                Name = provider.Name,
                Code = provider.Code,
                Contact = provider.Contact,
                Description = provider.Description,
                TaxId = provider.TaxId,
                Active = provider.Active
            };
        }         
    }
}
