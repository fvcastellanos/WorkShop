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
    public class ProviderService: ServiceBase
    {
        private readonly ILogger _logger;
        private readonly TokenProvider _tokenProvider;
        private readonly ProviderClient _providerClient;
        public ProviderService(ILogger<ProviderService> logger, 
                               ProviderClient providerClient,
                               TokenProvider tokenProvider,
                               IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, tokenProvider)
        {
            _logger = logger;
            _tokenProvider = tokenProvider;
            _providerClient = providerClient;
        }

        public Either<string, IEnumerable<ProviderView>> GetProviders(int topRows, string code, string name, int active)
        {
            try
            {
                _logger.LogInformation($"get top: {topRows} providers");

                var token = GetStrapiToken();

                return _providerClient.Find(token, topRows, code, name, active)
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
                var holder = FindById(view.Code);

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
                    Active = true
                };

                _providerClient.Add(GetStrapiToken(), provider);                
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
                var token = GetStrapiToken();
                var providerHolder = _providerClient.FindById(token, view.Id);
                var error = "";                

                providerHolder.Match(some => {
                    
                    var update = new Provider
                    {
                        Id = long.Parse(view.Id),
                        Name = view.Name,
                        Code = view.Code,
                        Contact = view.Contact,
                        TaxId = view.TaxId,
                        Description = view.Description,
                        Active = view.Active.Equals(1)
                    };

                    _providerClient.Update(token, update);
                }, () => {

                    error = $"Provider: {view.Name} not found";
                });

                if (!String.IsNullOrEmpty(error))
                {
                    return error;
                }

                return view;
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

                var token = GetStrapiToken();
                return _providerClient.FindById(token, id)
                    .Map(ToView);
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
                Active = provider.Active ? 1 : 0
            };
        }         
    }
}
