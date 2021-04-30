using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.Extensions.Logging;
using WorkShop.Domain;
using WorkShop.Model;

namespace WorkShop.Services
{
    public class ProviderInvoiceService
    {
        private readonly ILogger _logger;
        private readonly WorkShopContext _dbContext;

        public ProviderInvoiceService(ILogger<ProviderInvoiceService> logger, WorkShopContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Either<string, IEnumerable<ProviderInvoiceView>> GetInvoices(string code, string taxId = "", int top = 25)
        {
            try
            {
                return _dbContext.ProviderInvoices
                    .Where(pi => pi.Provider.Code.Equals(code)
                        && pi.Provider.TaxId.Contains(taxId))
                    .Select(ToView)
                    .Take(top)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get invoices for provider code: {code} - ", ex.Message);
                return $"Can't get Invoices for provider code: {code}";
            }
        }

        private ProviderInvoiceView ToView(ProviderInvoice providerInvoice)
        {
            return new ProviderInvoiceView()
            {
                Id = providerInvoice.Id.ToString(),
                Number = providerInvoice.Number,
                ProviderCode = providerInvoice.Provider.Code,
                ProviderId = providerInvoice.Provider.Id.ToString(),
                ProviderName = providerInvoice.Provider.Name,
                ProviderTaxId = providerInvoice.Provider.TaxId,
                ImageUrl = providerInvoice.ImageUrl
            };
        }
    }
}