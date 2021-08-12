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
    public class ProviderInvoiceService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly InvoiceRepository _invoiceRepository;

        private readonly ProductRepository _productRepository;

        private readonly ProviderRepository _providerRepository;

        public ProviderInvoiceService(ILogger<ProviderInvoiceService> logger,
                                      InvoiceRepository invoiceRepository,
                                      ProductRepository productRepository,
                                      ProviderRepository providerRepository)
        {
            _logger = logger;
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
            _providerRepository = providerRepository;
        }

        public Either<string, IEnumerable<InvoiceView>> GetInvoices(InvoiceSearchView searchView)
        {
            try
            {
                _logger.LogInformation($"Get top: {searchView.TopRows} invoices");

                return _invoiceRepository.FindInvoices(searchView.TopRows, searchView.Serial, searchView.Number, 
                    searchView.ProviderCode, searchView.ProviderName, searchView.Active)
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
                return _invoiceRepository.FindById(invoiceId)
                    .Map(ToView);
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

                var provider = _providerRepository.FindById(view.ProviderView.Id)
                    .FirstOrDefault();

                var invoice = new Invoice
                {
                    Serial = view.Serial,
                    Number = view.Number,
                    Description = view.Description,
                    Created = view.Created,
                    Active  = 1,
                    Kind = "Provider",
                    Type = "Cash",
                    Provider = provider,
                    Tenant = DefaultTenant
                };

                var storedInvoice = _invoiceRepository.Add(invoice);

                return ToView(storedInvoice);
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
                return _invoiceRepository.GetDetails(invoiceId)
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
                var invoice = _invoiceRepository.FindById(invoiceDetailView.InvoiceId)
                    .FirstOrDefault();

                var product = _productRepository.FindById(invoiceDetailView.ProductView.Id)
                    .FirstOrDefault();

                var detail = new InvoiceDetail
                {
                    Invoice = invoice,
                    Product = product,
                    Quantity = invoiceDetailView.Quantity,
                    Price = invoiceDetailView.Price,
                    DiscountAmount = invoiceDetailView.DiscountAmount,
                    Total = (invoiceDetailView.Quantity * invoiceDetailView.Price) - invoiceDetailView.DiscountAmount,
                    Created = DateTime.Now
                };

                var storedDetail = _invoiceRepository.AddDetail(detail);

                UpdateInvoiceAmount(invoiceDetailView.InvoiceId);
                // AddInventoryMovement()

                return ToDetailView(storedDetail);
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
                var detail = _invoiceRepository.FindDetailById(view.Id)
                    .FirstOrDefault();
                    
                var product = _productRepository.FindById(view.ProductView.Id)
                    .FirstOrDefault();

                detail.Quantity = view.Quantity;
                detail.Price = view.Price;
                detail.DiscountAmount = view.DiscountAmount;
                detail.Total = (view.Price * view.Quantity) - view.DiscountAmount;
                detail.Product = product;

                _invoiceRepository.UpdateDetail(detail);

                UpdateInvoiceAmount(view.InvoiceId);
                // UpdateInventoryMovement()

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
                var detailHolder = _invoiceRepository.FindDetailById(id);

                if (detailHolder.IsSome)
                {
                    var detail = detailHolder.FirstOrDefault();

                    _invoiceRepository.DeleteDetail(detail);
                    UpdateInvoiceAmount(detail.Invoice.Id.ToString());
                    // UpdateInvoentory()

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
                return _invoiceRepository.FindDetailById(detailId)
                    .Map(ToDetailView);
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
                ImageUrl = invoice.ImageUrl,
                Total = invoice.Total,                
                ProviderView = new ProviderView
                {
                    Id = invoice.Provider.Id.ToString(),
                    Name = invoice.Provider.Name,
                    Code = invoice.Provider.Code,
                    TaxId = invoice.Provider.TaxId,
                }
            };
        }

        private InvoiceDetailView ToDetailView(InvoiceDetail detail)
        {
            return new InvoiceDetailView
            {
                Id = detail.Id.ToString(),
                Quantity = detail.Quantity,
                Price = detail.Price,
                DiscountAmount = detail.DiscountAmount,
                Total = (detail.Quantity * detail.Price) - detail.DiscountAmount,
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
            var holder = _invoiceRepository.FindByInvoiceNumber(invoiceView.ProviderView.Id, invoiceView.Serial, invoiceView.Number);

            return holder.IsSome;
        }

        private void UpdateInvoiceAmount(string invoiceId)
        {
            var invoiceHolder = _invoiceRepository.FindById(invoiceId);

            if (invoiceHolder.IsSome)
            {
                var invoice = invoiceHolder.FirstOrDefault();

                invoice.Total = _invoiceRepository.GetDetails(invoiceId)
                    .Sum(detail => (detail.Quantity * detail.Price) - detail.DiscountAmount);

                _invoiceRepository.Update(invoice);
            }

            // var invoice = _dbContext.Invoices.Find(Guid.Parse(invoiceId));

            // invoice.Total = _dbContext.InvoiceDetails
            //     .Where(detail => detail.Invoice.Id.Equals(Guid.Parse(invoiceId)))
            //     .Sum(detail => (detail.Quantity * detail.Price) - detail.DiscountAmount);

            // _dbContext.Invoices.Update(invoice);
            // _dbContext.SaveChanges();
        }
    }
}