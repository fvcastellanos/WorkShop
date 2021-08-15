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
    public class CustomerService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly CustomerRepository _repository;

        public CustomerService(ILogger<CustomerService> logger,
                               CustomerRepository customerRepository)
        {
            _logger = logger;
            _repository = customerRepository;
        }

        public Either<string, IEnumerable<CustomerView>> FindCustomers(CustomerSearchView searchView)
        {
            try
            {
                _logger.LogInformation($"Getting top rows: {searchView.TopRows} customers");

                return _repository.FindCustomers(searchView.TopRows, searchView.Code, searchView.Name, searchView.TaxId, searchView.Active)
                    .Select(ToView)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get customer list - {ex.Message}");
                return "No se puede obtener el listado de clientes";
            }
        }

        public Option<CustomerView> FindById(string id)
        {
            try
            {
                return _repository.FindById(id)
                    .Map(ToView);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get customer with Id: {id} - {ex.Message}");
                return null;
            }
        }

        public Either<string, CustomerView> Add(CustomerView view)
        {
            try
            {
                var customerHolder = _repository.FindByCode(view.Code);

                if (customerHolder.IsSome)
                {
                    _logger.LogError($"Customer code: {view.Code} already exists");
                    return $"El código: {view.Code} ya se encuentra en uso";
                }

                var customer = new Customer
                {
                    Code = view.Code,
                    Name = view.Name,
                    TaxId = view.TaxId,
                    Description = view.Description,
                    Active = 1,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Tenant = DefaultTenant
                };

                var storedCustomer = _repository.Add(customer);

                return ToView(storedCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't add customer with code: {view.Name} - {ex.Message}");
                return $"No se puede agregar el cliente con nombre: {view.Name}";
            }
        }

        public Either<string, CustomerView> Update(CustomerView view)
        {
            try
            {
                var customerHolder = _repository.FindById(view.Id);

                if (customerHolder.IsNone)
                {
                    _logger.LogError($"Customer with Id: {view.Id} not found");
                    return $"No se encontró el cliente con Id: {view.Id}";
                }

                var customer = customerHolder.FirstOrDefault();

                customer.Code = view.Code;
                customer.Name = view.Name;
                customer.TaxId = view.TaxId;
                customer.Description = view.Description;
                customer.Active = view.Active;
                customer.Updated = DateTime.Now;

                var storedCustomer = _repository.Update(customer);

                return ToView(storedCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cant update customer with code: {view.Code} - {ex.Message}");
                return $"No es posible actualizar el cliente con código: ${view.Code}";
            }
        }

        // -------------------------------------------------------------------------------------------------------------------

        private CustomerView ToView(Customer customer)
        {
            return new CustomerView
            {
                Id = customer.Id.ToString(),
                Code = customer.Code,
                Name = customer.Name,
                TaxId = customer.TaxId,
                Description = customer.Description,
                Active = customer.Active
            };
        }
    }
}