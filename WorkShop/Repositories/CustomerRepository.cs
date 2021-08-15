using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using WorkShop.Model;

namespace WorkShop.Repositories
{
    public class CustomerRepository
    {
        private readonly WorkShopContext _dbContext;

        public CustomerRepository(WorkShopContext workShopContext)
        {
            _dbContext = workShopContext;
        }

        public IEnumerable<Customer> FindCustomers(int top, string code, string name, string taxId, int active)
        {
            return _dbContext.Customers.Where(customer => customer.Active.Equals(active) &&
                    customer.Code.Contains(code, StringComparison.CurrentCultureIgnoreCase) &&
                    customer.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase) &&
                    customer.TaxId.Contains(taxId, StringComparison.CurrentCultureIgnoreCase))
                .Take(top)
                .ToList();                            
        }

        public Option<Customer> FindByCode(string code)
        {
            return _dbContext.Customers.FirstOrDefault(customer => customer.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));
        }

        public Option<Customer> FindById(string id)
        {
            return _dbContext.Customers.Find(Guid.Parse(id));
        }

        public Customer Add(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();

            return customer;
        }

        public Customer Update(Customer customer)
        {
            _dbContext.Customers.Update(customer);
            _dbContext.SaveChanges();

            return customer;
        }

    }
}