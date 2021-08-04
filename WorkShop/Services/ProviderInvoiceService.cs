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
    public class ProviderInvoiceService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly WorkShopContext _dbContext;

        private readonly ProviderInvoiceClient _providerInvoiceClient;

        public ProviderInvoiceService(IHttpContextAccessor httpContextAccessor, 
                                      TokenProvider tokenProvider,
                                      ILogger<ProviderInvoiceService> logger,
                                      WorkShopContext workShopContext,
                                      ProviderInvoiceClient providerInvoiceClient) : base(httpContextAccessor, tokenProvider)
        {
            _providerInvoiceClient = providerInvoiceClient;
            _logger = logger;
            _dbContext = workShopContext;
        }

        public Either<string, IEnumerable<ProviderInvoiceView>> GetInvoices(string providerId, 
                                                                            string serial = "",
                                                                            string number = "", 
                                                                            int active = 1, 
                                                                            int top = 25)
        {
            try
            {
                return _dbContext.ProviderInvoices.Where(invoice => invoice.Active.Equals(active) 
                    && invoice.Provider.Id.Equals(Guid.Parse(providerId))
                    && invoice.Serial.Contains(serial) && invoice.Number.Contains(number))
                    .Take(top)
                    .Select(ToView)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get invoices for provider id: {providerId} - ", ex.Message);
                return $"Can't get Invoices for provider id: {providerId}";
            }
        }

        public Option<ProviderInvoiceView> GetInvoice(string invoiceId)
        {
            try
            {
                return _dbContext.ProviderInvoices.Where(invoice => invoice.Id.Equals(Guid.Parse(invoiceId)))
                    .Map(ToView)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get invoice with id: {invoiceId} - ", ex.Message);
                return null;
            }
        }

        public Either<string, ProviderInvoiceView> Add(string providerId, ProviderInvoiceView view)
        {
            try
            {
                if (InvoiceExists(providerId, view.Serial, view.Number))
                {
                    return $"Invoice: {view.Number} already exists for Provider: {providerId}";
                }

                var providerInvoice = new ProviderInvoice
                {
                    Number = view.Number,
                    Amount = view.Amount,
                    Serial = view.Serial,
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

        public Either<string, ProviderInvoiceView> Update(long providerId, ProviderInvoiceView view)
        {
            try
            {
                var token = GetStrapiToken();
                var holder = _providerInvoiceClient.FindById(token, view.Id);
                var error = "";

                holder.Match(some => {

                    var provider = new ProviderInvoice
                    {
                        Id = long.Parse(view.Id),
                        Serial = view.Serial,
                        Number = view.Number,
                        Created = view.Created,
                        Amount = view.Amount,
                        Description = view.Description,
                        Active = view.Active.Equals(1),
                        Provider = new Provider
                        {
                            Id = providerId
                        }
                    };

                    _providerInvoiceClient.Update(token, provider);

                }, () => error = $"Invoice: {view.Number} not found");

                if (string.IsNullOrEmpty(error))
                {
                    return view;
                }

                return error;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't update invoice number: {view.Number} for provider: {providerId} - ", ex.Message);
                return $"Can't update invoice number: {view.Number} for provider: {providerId}";
            }
        }

        private ProviderInvoiceView ToView(ProviderInvoice providerInvoice)
        {
            return new ProviderInvoiceView()
            {
                Id = providerInvoice.Id.ToString(),
                Number = providerInvoice.Number,
                Serial = providerInvoice.Serial,
                Active = providerInvoice.Active ? 1 : 0,
                Description = providerInvoice.Description,
                Amount = providerInvoice.Amount,
                Created = providerInvoice.Created
                // ImageUrl = providerInvoice.ImageUrl
            };
        }



        private bool InvoiceExists(string providerId, string serial, string number)
        {
            var invoice = _dbContext.ProviderInvoices.Where(invoice => invoice.Provider.Id.Equals(Guid.Parse(providerId)) 
                && invoice.Serial.Equals(serial) && invoice.Number.Equals(number))
                .FirstOrDefault();

            return invoice == null;
            // var holder = _providerInvoiceClient.FindByNumber(GetStrapiToken(), providerId, suffix, number);
            
            // return holder.IsSome;
        }
    }
}