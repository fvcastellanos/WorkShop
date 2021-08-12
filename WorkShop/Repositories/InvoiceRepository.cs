using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using WorkShop.Model;

namespace WorkShop.Repositories
{
    public class InvoiceRepository
    {
        private readonly WorkShopContext _dbContext;

        public InvoiceRepository(WorkShopContext workShopContext)
        {
            _dbContext = workShopContext;
        }

        public IEnumerable<Invoice> FindInvoices(int top,
                                                 string serial,
                                                 string number,
                                                 string providerCode,
                                                 string providerName,
                                                 int active)
        {
            return _dbContext.Invoices.Where(invoice => invoice.Active.Equals(active) 
                    && invoice.Provider.Code.Contains(providerCode)
                    && invoice.Provider.Name.Contains(providerName)
                    && invoice.Serial.Contains(serial) 
                    && invoice.Number.Contains(number))
                .OrderByDescending(invoice => invoice.Created)
                .Take(top)
                .ToList();            
        }

        public Option<Invoice> FindById(string id)
        {
            return _dbContext.Invoices.Where(invoice => invoice.Id.Equals(Guid.Parse(id)))
                .FirstOrDefault();        
        }

        public Option<Invoice> FindByInvoiceNumber(string providerId, string serial, string number)
        {
            return _dbContext.Invoices.Where(invoice => 
                invoice.Provider.Id.Equals(Guid.Parse(providerId)) 
                    && invoice.Serial.Equals(serial) 
                    && invoice.Number.Equals(number))
                .FirstOrDefault();            
        }

        public Invoice Add(Invoice invoice)
        {
            _dbContext.Invoices.Add(invoice);
            _dbContext.SaveChanges();

            return invoice;
        }

        public Invoice Update(Invoice invoice)
        {
            _dbContext.Invoices.Update(invoice);
            _dbContext.SaveChanges();

            return invoice;
        }

        public void Delete(Invoice invoice)
        {
            _dbContext.Invoices.Remove(invoice);
            _dbContext.SaveChanges();
        }

        public IEnumerable<InvoiceDetail> GetDetails(string invoiceId)
        {
            return _dbContext.InvoiceDetails.Where(detail => detail.Invoice.Id.Equals(Guid.Parse(invoiceId)))
                .ToList();
        }

        public InvoiceDetail AddDetail(InvoiceDetail invoiceDetail)
        {
            _dbContext.InvoiceDetails.Add(invoiceDetail);
            _dbContext.SaveChanges();

            return invoiceDetail;
        }

        public InvoiceDetail UpdateDetail(InvoiceDetail invoiceDetail)
        {
            _dbContext.InvoiceDetails.Update(invoiceDetail);
            _dbContext.SaveChanges();

            return invoiceDetail;
        }

        public InvoiceDetail DeleteDetail(InvoiceDetail invoiceDetail)
        {
            _dbContext.InvoiceDetails.Remove(invoiceDetail);
            _dbContext.SaveChanges();

            return invoiceDetail;
        }

        public Option<InvoiceDetail> FindDetailById(string detailId)
        {
            return _dbContext.InvoiceDetails
                .FirstOrDefault(detail => detail.Id.Equals(Guid.Parse(detailId)));
        }
    }
}