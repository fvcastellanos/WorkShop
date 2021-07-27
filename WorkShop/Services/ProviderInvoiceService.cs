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
    public class ProviderInvoiceService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly ProviderInvoiceClient _providerInvoiceClient;

        public ProviderInvoiceService(IHttpContextAccessor httpContextAccessor, 
                                      TokenProvider tokenProvider,
                                      ILogger<ProviderInvoiceService> logger,
                                      ProviderInvoiceClient providerInvoiceClient) : base(httpContextAccessor, tokenProvider)
        {
            _providerInvoiceClient = providerInvoiceClient;
            _logger = logger;
        }

        public Either<string, IEnumerable<ProviderInvoiceView>> GetInvoices(long providerId, string number = "", int active = 1, int top = 25)
        {
            try
            {
                return _providerInvoiceClient.Find(GetStrapiToken(), top, providerId, number, active)
                    .Map(ToView)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get invoices for provider id: {providerId} - ", ex.Message);
                return $"Can't get Invoices for provider id: {providerId}";
            }
        }

        public Either<string, ProviderInvoiceView> Add(long providerId, ProviderInvoiceView view)
        {
            try
            {
                var providerInvoice = new ProviderInvoice
                {
                    Number = view.Number,
                    Amount = view.Amount,
                    Suffix = view.Suffix,
                    Description = view.Description,
                    Created = view.Created,
                    Active  = true,
                    Provider = new Provider
                    {
                        Id = providerId
                    }
                };

                _providerInvoiceClient.Add(GetStrapiToken(), providerInvoice);

                return view;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't add invoice number {view.Number} for provider: {providerId} - ", ex.Message);
                return $"Can't add invoice number {view.Number} for provider: {providerId}";
            }
        }

        private ProviderInvoiceView ToView(ProviderInvoice providerInvoice)
        {
            return new ProviderInvoiceView()
            {
                Id = providerInvoice.Id.ToString(),
                Number = providerInvoice.Number,
                Suffix = providerInvoice.Suffix,
                Active = providerInvoice.Active ? 1 : 0,
                Description = providerInvoice.Description,
                Amount = providerInvoice.Amount,
                Created = providerInvoice.Created
                // ImageUrl = providerInvoice.ImageUrl
            };
        }
    }
}