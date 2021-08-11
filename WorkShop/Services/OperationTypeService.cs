using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.Extensions.Logging;
using WorkShop.Domain;
using WorkShop.Model;

namespace WorkShop.Services
{
    public class OperationTypeService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly WorkShopContext _dbContext;

        public OperationTypeService(ILogger<OperationTypeService> logger, 
                                    WorkShopContext workShopContext)
        {
            _logger = logger;
            _dbContext = workShopContext;
        }

        public Option<OperationTypeView> GetById(string id)
        {
            try
            {
                _logger.LogInformation($"load operation type with id: {id}");

                return _dbContext.OperationTypes.Where(operationType => operationType.Id.Equals(Guid.Parse(id)))
                    .Map(ToView)
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

                return _dbContext.OperationTypes.Where(operationType => operationType.Active.Equals(active) && 
                    operationType.Name.Contains(name))
                    .Take(top)
                    .Select(ToView)
                    .ToList();

                // return _operationTypeClient.Find(GetStrapiToken(), top, name, active)
                //     .Select(ToView)
                //     .ToList();
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

                var operationType = new OperationType
                {
                    Name = view.Name,
                    Description = view.Description,
                    Active = 1,
                    Inbound = view.Inbound,
                    Created = DateTime.Now,
                    Tenant = DefaultTenant
                };

                _dbContext.OperationTypes.Add(operationType);
                _dbContext.SaveChanges();

                _logger.LogInformation($"Add new operation type with name: {view.Name}");
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
                var operationType = _dbContext.OperationTypes.Find(Guid.Parse(view.Id));

                if (operationType == null)
                {
                    return $"Operation Type: {view.Name} not found";
                }

                operationType.Name = view.Name;
                operationType.Description = view.Description;
                operationType.Inbound = view.Inbound;
                operationType.Active = view.Active;
                operationType.Updated = DateTime.Now;

                _dbContext.OperationTypes.Update(operationType);
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
                return _dbContext.OperationTypes.Where(operationType => 
                    operationType.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
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