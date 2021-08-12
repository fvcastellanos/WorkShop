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
    public class ProviderService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly ProviderRepository _providerRepository;

        public ProviderService(ILogger<ProviderService> logger, 
                               ProviderRepository providerRepository)
        {
            _logger = logger;
            _providerRepository = providerRepository;
        }

        public Either<string, IEnumerable<ProviderView>> GetProviders(int topRows, string code, string name, int active)
        {
            try
            {                
                _logger.LogInformation($"get top: {topRows} providers");

                return _providerRepository.FindProviders(topRows, code, name, active)
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
                var holder = _providerRepository.FindByCode(view.Code);

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

                var storedProvider = _providerRepository.Add(provider);

                return ToView(storedProvider);
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
                var providerHolder = _providerRepository.FindById(view.Id);

                if (providerHolder.IsNone)
                {
                    return $"Provider: {view.Name} not found";
                }

                var provider = providerHolder.FirstOrDefault();
                provider.Code = view.Code;
                provider.Name = view.Name;
                provider.Contact = view.Contact;
                provider.Description = view.Description;
                provider.TaxId = view.TaxId;
                provider.Active = view.Active;
                provider.Updated = DateTime.Now;

                var storedProvider = _providerRepository.Update(provider);

                return ToView(storedProvider);
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't update provider: {view.Name} - ", ex.Message);
                return $"Can't update provider: {view.Name}";
            }
        }

        public Option<ProviderView> FindById(string id)
        {
            try
            {
                _logger.LogInformation($"looking for provider with id: {id}");

                return _providerRepository.FindById(id)
                    .Map(ToView)
                    .FirstOrDefault();                    
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't find provider with id {id} - {ex.Message}");
                return null;
            }
        }

        public IEnumerable<ProviderView> GetActiveProviders()
        {
            try 
            {
                _logger.LogInformation("Load active providers");
                return _providerRepository.GetActiveProviders()
                    .Select(ToView)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't get active providers - ", ex.Message);
                return new List<ProviderView>();
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
