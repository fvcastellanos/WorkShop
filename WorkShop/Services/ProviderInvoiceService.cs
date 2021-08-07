using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.Extensions.Logging;
using WorkShop.Domain;
using WorkShop.Model;

namespace WorkShop.Services
{
    public class ProviderInvoiceService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly WorkShopContext _dbContext;

        public ProviderInvoiceService(ILogger<ProviderInvoiceService> logger,
                                      WorkShopContext workShopContext)
        {
            _logger = logger;
            _dbContext = workShopContext;
        }

        public Either<string, IEnumerable<InvoiceView>> GetInvoices(InvoiceSearchView searchView)
        {
            try
            {
                _logger.LogInformation($"Get top: {searchView.TopRows} invoices");

                return _dbContext.Invoices.Where(invoice => invoice.Active.Equals(searchView.Active) 
                        && invoice.Provider.Code.Contains(searchView.ProviderCode)
                        && invoice.Provider.Code.Contains(searchView.ProviderName)
                        && invoice.Serial.Contains(searchView.Serial) 
                        && invoice.Number.Contains(searchView.Number))
                    .Take(searchView.TopRows)
                    .Select(ToView)
                    .ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get invoices - ", ex.Message);
                return "Can't get Invoices";
            }
        }

        public Option<InvoiceView> GetInvoice(string invoiceId)
        {
            try
            {
                return _dbContext.Invoices.Where(invoice => invoice.Id.Equals(Guid.Parse(invoiceId)))
                    .Map(ToView)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get invoice with id: {invoiceId} - ", ex.Message);
                return null;
            }
        }

        public Either<string, InvoiceView> Add(InvoiceView view)
        {
            try
            {
                if (InvoiceExists(view))
                {
                    return $"Invoice: {view.Number} already exists for Provider: {view.ProviderView.Name}";
                }

                var provider = _dbContext.Providers.Find(Guid.Parse(view.ProviderView.Id));

                var invoice = new Invoice
                {
                    Serial = view.Serial,
                    Number = view.Number,
                    // Amount = view.Amount,
                    Description = view.Description,
                    Created = view.Created,
                    Active  = 1,
                    Kind = "Provider",
                    Type = "Cash",
                    Provider = provider,
                    Tenant = DefaultTenant
                };

                _dbContext.Invoices.Add(invoice);
                _dbContext.SaveChanges();

                return view;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't add invoice number {view.Number} for provider: {view.ProviderView.Name} - {ex.Message}");
                return $"Can't add invoice number {view.Number} for provider: {view.ProviderView.Name}";
            }
        }

        public Either<string, InvoiceView> Update(long providerId, InvoiceView view)
        {
            try
            {
                return view;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't update invoice number: {view.Number} for provider: {providerId} - {ex.Message}");
                return $"Can't update invoice number: {view.Number} for provider: {providerId}";
            }
        }

        private InvoiceView ToView(Invoice invoice)
        {
            return new InvoiceView()
            {
                Id = invoice.Id.ToString(),
                Serial = invoice.Serial,
                Number = invoice.Number,
                Active = invoice.Active,
                Created = invoice.Created,
                ProviderView = new ProviderView
                {
                    Id = invoice.Provider.Id.ToString(),
                    Name = invoice.Provider.Name,
                    Code = invoice.Provider.Code,
                    TaxId = invoice.Provider.TaxId,
                },
                // ImageUrl = providerInvoice.ImageUrl
            };
        }

        private bool InvoiceExists(InvoiceView invoiceView)
        {
            var invoice = _dbContext.Invoices.Where(invoice => 
                    invoice.Provider.Id.Equals(Guid.Parse(invoiceView.ProviderView.Id)) 
                    && invoice.Serial.Equals(invoiceView.Serial) 
                    && invoice.Number.Equals(invoiceView.Number))
                .FirstOrDefault();

            return invoice != null;
        }
    }
}