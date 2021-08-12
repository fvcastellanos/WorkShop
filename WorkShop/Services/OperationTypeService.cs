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
    public class OperationTypeService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly OperationTypeRepository _operationTypeRepository;

        public OperationTypeService(ILogger<OperationTypeService> logger, 
                                    OperationTypeRepository operationTypeRepository)
        {
            _logger = logger;
            _operationTypeRepository = operationTypeRepository;
        }

        public Option<OperationTypeView> GetById(string id)
        {
            try
            {
                _logger.LogInformation($"load operation type with id: {id}");

                return _operationTypeRepository.FindById(id)
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

                return _operationTypeRepository.FindOperationTypes(top, name, active)
                    .Select(ToView)
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
                var operationTypeHolder = _operationTypeRepository.FindByName(view.Name);

                if (operationTypeHolder.IsSome)
                {
                    return $"Operation Type with name: {view.Name} already exists";
                }

                _logger.LogInformation($"Add new operation type with name: {view.Name}");

                var operationType = new OperationType
                {
                    Name = view.Name,
                    Description = view.Description,
                    Active = 1,
                    Inbound = view.Inbound,
                    Created = DateTime.Now,
                    Tenant = DefaultTenant
                };

                var storedEntity = _operationTypeRepository.Add(operationType);

                return ToView(storedEntity);

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
                var operationTypeHolder = _operationTypeRepository.FindById(view.Id);

                if (operationTypeHolder.IsNone)
                {
                    return $"Operation Type: {view.Name} not found";
                }

                var operationType = operationTypeHolder.FirstOrDefault();
                operationType.Name = view.Name;
                operationType.Description = view.Description;
                operationType.Inbound = view.Inbound;
                operationType.Active = view.Active;
                operationType.Updated = DateTime.Now;

                var storedEntity = _operationTypeRepository.Update(operationType);

                return ToView(storedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"can't update operation type: {view.Name} - ", ex.Message);
                return $"Can't update operation type: {view.Name}";
            }
        }

        // ---------------------------------------------------------------------------------------------------------

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
