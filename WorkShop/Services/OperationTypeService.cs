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

                var id = Guid.NewGuid().ToString();
                var operationType = new OperationType()
                {
                    Id = id,
                    Name = view.Name,
                    Description = view.Description,
                    Active = view.Active,
                    Tenant = DefaultTenant
                };

                _dbContext.OperationTypes.Add(operationType);
                _dbContext.SaveChanges();

                _logger.LogInformation($"Add new operation type with name: {view.Name}");

                view.Id = operationType.Id;
                return view;

            }
            catch (Exception exception)
            {
                _logger.LogError($"can't add operation type: {view.Name} - ", exception.Message);
                return $"Can't add operation type: {view.Name}";
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
                Id = operationType.Id,
                Name = operationType.Name,
                Description = operationType.Description,
                Active = operationType.Active
            };
        }

        private OperationType ToModel(OperationTypeView view)
        {
            return new OperationType()
            {
                Name = view.Name,
                Description = view.Description,
                Active = view.Active
            };
        }

    }
}