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

        public Either<string, IEnumerable<InvoiceDetailView>> GetInvoiceDetails(string invoiceId)
        {
            try
            {
                return _dbContext.InvoiceDetails.Where(detail => detail.Invoice.Id.Equals(Guid.Parse(invoiceId)))
                    .Select(ToDetailView)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get details for invoice: {invoiceId} - {ex.Message}");
                return $"Can't get details for invoice: {invoiceId}";
            }
        }

        public Either<string, InvoiceDetailView> AddDetail(InvoiceDetailView invoiceDetailView)
        {
            try
            {
                var invoice = _dbContext.Invoices.Find(Guid.Parse(invoiceDetailView.InvoiceId));
                var product = _dbContext.Products.Find(Guid.Parse(invoiceDetailView.ProductView.Id));

                var detail = new InvoiceDetail
                {
                    Invoice = invoice,
                    Product = product,
                    Quantity = invoiceDetailView.Amount,
                    Price = invoiceDetailView.Price,
                    Total = invoiceDetailView.Amount * invoiceDetailView.Price,
                    Created = DateTime.Now
                };

                _dbContext.InvoiceDetails.Add(detail);
                _dbContext.SaveChanges();

                return invoiceDetailView;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't add detail for invoice: {invoiceDetailView.InvoiceId} - {ex.Message}");
                return $"Can't get details for invoice: {invoiceDetailView.InvoiceId}";
            }
        }

        public Either<string, InvoiceDetailView> UpdateDetail(InvoiceDetailView view)
        {
            try
            {
                var detail = _dbContext.InvoiceDetails.Find(Guid.Parse(view.Id));
                var product = _dbContext.Products.Find(Guid.Parse(view.ProductView.Id));

                detail.Quantity = view.Amount;
                detail.Price = view.Price;
                detail.Total = view.Price * view.Amount;
                detail.Product = product;

                _dbContext.InvoiceDetails.Update(detail);
                _dbContext.SaveChanges();

                return view;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't update detail for invoice: {view.InvoiceId} - {ex.Message}");
                return $"Can't update detail for invoice: {view.InvoiceId}";
            }
        }

        public Either<string, int> DeleteDetail(string id)
        {
            try
            {
                var detail = _dbContext.InvoiceDetails.Find(Guid.Parse(id));

                if (detail != null)
                {
                    _dbContext.InvoiceDetails.Remove(detail);
                    _dbContext.SaveChanges();

                    return 1;
                }
                
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't deelte detail id: {id} - {ex.Message}");
                return $"Can't delete detail id: {id}";
            }
        }

        public Option<InvoiceDetailView> GetDetail(string detailId)
        {
            try
            {
                return _dbContext.InvoiceDetails.Where(detail => detail.Id.Equals(Guid.Parse(detailId)))
                    .Map(ToDetailView)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get invoice detail id: {detailId} - {ex.Message}");
                return null;
            }
        }

        // --------------------------------------------------------------------------------------------------------------

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

        private InvoiceDetailView ToDetailView(InvoiceDetail detail)
        {
            return new InvoiceDetailView
            {
                Id = detail.Id.ToString(),
                Amount = detail.Quantity,
                Price = detail.Price,
                Total = detail.Quantity * detail.Price,
                InvoiceId = detail.Invoice.Id.ToString(),
                ProductView = new ProductView
                {
                    Id = detail.Product.Id.ToString(),
                    Code = detail.Product.Code,
                    Name = detail.Product.Name,
                    Active = detail.Product.Active
                }
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