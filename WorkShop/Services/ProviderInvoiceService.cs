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

        public Option<ProviderInvoiceView> GetInvoice(string invoiceId)
        {
            try
            {
                 return _providerInvoiceClient.FindById(GetStrapiToken(), invoiceId)
                    .Map(ToView);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get invoice with id: {invoiceId} - ", ex.Message);
                return null;
            }
        }

        public Either<string, ProviderInvoiceView> Add(long providerId, ProviderInvoiceView view)
        {
            try
            {
                if (InvoiceExists(providerId, view.Suffix, view.Number))
                {
                    return $"Invoice: {view.Number} already exists for Provider: {providerId}";
                }

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
                        Suffix = view.Suffix,
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
                Suffix = providerInvoice.Suffix,
                Active = providerInvoice.Active ? 1 : 0,
                Description = providerInvoice.Description,
                Amount = providerInvoice.Amount,
                Created = providerInvoice.Created
                // ImageUrl = providerInvoice.ImageUrl
            };
        }



        private bool InvoiceExists(long providerId, string suffix, string number)
        {

            var holder = _providerInvoiceClient.FindByNumber(GetStrapiToken(), providerId, suffix, number);
            
            return holder.IsSome;
        }
    }
}