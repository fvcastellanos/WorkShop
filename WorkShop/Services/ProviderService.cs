using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using WorkShop.Clients;
using WorkShop.Domain;
using WorkShop.Model;

namespace WorkShop.Services
{
    public class ProviderService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly WorkShopContext _dbContext;

        private readonly ProviderClient _providerClient;

        public ProviderService(ILogger<ProviderService> logger, 
                               WorkShopContext dbContext, 
                               ProviderClient providerClient,
                               IJSRuntime jSRuntime,
                               IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, jSRuntime)
        {
            _logger = logger;
            _dbContext = dbContext;
            _providerClient = providerClient;
        }


        public Either<string, IEnumerable<ProviderView>> GetProviders(int topRows, string code, string name, int active)
        {
            try
            {
                _logger.LogInformation($"get top: {topRows} providers");

                var token = GetStrapiToken();

                return _providerClient.Find(token, topRows, code, name)
                    .Select(ToView)
                    .ToList();

                // return _dbContext.Providers
                //     .Where(p => p.Active == active
                //         && p.Code.Contains(code)
                //         && p.Name.Contains(name))
                //     .Select(ToView)
                //     .Take(topRows)
                //     .ToList();
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

                var provider = new Provider()
                {
                    Code = view.Code,
                    Name = view.Name,
                    Contact = view.Contact,
                    TaxId = view.TaxId,
                    Description = view.Description,
                    Tenant = DefaultTenant
                };

                _logger.LogInformation($"Add new provider: {view.Name}");
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
                var id = Guid.Parse(view.Id);
                var provider = _dbContext.Providers.Find(id);

                if (provider == null)
                {
                    return $"Provider: {view.Name} not found";
                }

                provider.Name = view.Name;
                provider.Code = view.Code;
                provider.Contact = view.Contact;
                provider.TaxId = view.TaxId;
                provider.Description = view.Description;
                provider.Active = view.Active;

                _dbContext.Providers.Update(provider);
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

                return _dbContext.Providers
                    .Where(p => p.Code.Equals(code))
                    .Select(ToView)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't find provider with code {code} - ", ex.Message);
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

       private ProviderView ToView(WorkShop.Clients.Domain.Provider provider)
        {
            return new ProviderView()
            {
                Id = provider.Id.ToString(),
                Name = provider.Name,
                Code = provider.Code,
                Contact = provider.Contact,
                Description = provider.Description,
                TaxId = provider.TaxId,
            };
        }
         
    }
}