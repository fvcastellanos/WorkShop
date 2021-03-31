using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.Extensions.Logging;
using WorkShop.Domain;
using WorkShop.Model;

namespace WorkShop.Services
{
    public class OperationTypeService
    {
        private const string DefaultTenant = "default";

        private readonly ILogger _logger;

        private readonly WorkShopContext _dbContext;

        public OperationTypeService(ILogger<OperationTypeService> logger, WorkShopContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Option<OperationTypeView> GetById(string id)
        {
            try
            {
                _logger.LogInformation($"load operation type with id: {id}");

                var key = Guid.Parse(id);

                return _dbContext.OperationTypes
                    .Where(ot => ot.Id.Equals(key))
                    .Select(ToView)
                    .FirstOrDefault();
            }
            catch (Exception exception)
            {
                _logger.LogError($"can't load operation type with id: {id} - ", exception);
                return null;
            }
        }

        public Either<string, IEnumerable<OperationTypeView>> GetOperationTypes(int top = 25, string name = "", int active = 1)
        {
            try
            {
                _logger.LogInformation($"Get top {top} operation types");

                return _dbContext.OperationTypes
                    .Where(row => row.Active.Equals(active) 
                        && row.Name.Contains(name))
                    .Select(ToView)
                    .Take(top)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't get operation types - ", ex);
                return "Can't get operation types";
            }
        }

        public Either<string, OperationTypeView> Add(OperationTypeView view)
        {
            try
            {
                var operationTypeHolder = FindByName(view.Name);

                if (operationTypeHolder.IsSome)
                {
                    return $"Operation Type with name: {view.Name} already exists";
                }

                var operationType = new OperationType()
                {
                    Name = view.Name,
                    Description = view.Description,
                    Active = view.Active,
                    Inbound = view.Inbound,
                    Tenant = DefaultTenant
                };

                _dbContext.OperationTypes.Add(operationType);
                _dbContext.SaveChanges();

                _logger.LogInformation($"Add new operation type with name: {view.Name}");

                view.Id = operationType.Id.ToString();
                return view;

            }
            catch (Exception exception)
            {
                _logger.LogError($"can't add operation type: {view.Name} - ", exception.Message);
                return $"Can't add operation type: {view.Name}";
            }
        }

        public Either<string, OperationTypeView> Update(OperationTypeView view)
        {
            try
            {
                var id = Guid.Parse(view.Id);
                var model = _dbContext.OperationTypes.Find(id);

                if (model == null)
                {
                    return $"Operation Type: {view.Name} not found";
                }

                model.Name = view.Name;
                model.Description = view.Description;
                model.Inbound = view.Inbound;
                model.Active = view.Active;
                model.Updated = DateTime.Now;

                _logger.LogInformation($"Update operation type: {view.Name}");

                _dbContext.OperationTypes.Update(model);
                _dbContext.SaveChanges();

                return view;
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't update operation type: {view.Name} - ", ex.Message);
                return $"Can't update operation type: {view.Name}";
            }
        }

        // ---------------------------------------------------------------------------------------------------------

        private Option<OperationTypeView> FindByName(string name)
        {
            try
            {
                return _dbContext.OperationTypes
                    .Where(ot => ot.Name.Equals(name))
                    .Select(ToView)
                    .FirstOrDefault();
            }
            catch (Exception exception)
            {
                _logger.LogError($"can't find operation type with name: {name} - ", exception.Message);
                return null;
            }
        }

        private OperationTypeView ToView(OperationType operationType)
        {
            return new OperationTypeView()
            {
                Id = operationType.Id.ToString(),
                Name = operationType.Name,
                Description = operationType.Description,
                Active = operationType.Active,
                Inbound = operationType.Inbound
            };
        }
    }
}