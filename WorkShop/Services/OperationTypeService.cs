using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WorkShop.Clients;
using WorkShop.Clients.Domain;
using WorkShop.Domain;
using WorkShop.Providers;

namespace WorkShop.Services
{
    public class OperationTypeService: ServiceBase
    {
        private readonly ILogger _logger;

        private readonly OperationTypeClient _operationTypeClient;

        public OperationTypeService(ILogger<OperationTypeService> logger, 
                                    OperationTypeClient operationTypeClient,
                                    TokenProvider tokenProvider,
                                    IHttpContextAccessor httpContextAccessor): base(httpContextAccessor, tokenProvider)
        {
            _logger = logger;
            _operationTypeClient = operationTypeClient;

        }

        public Option<OperationTypeView> GetById(string id)
        {
            try
            {
                _logger.LogInformation($"load operation type with id: {id}");
                
                return _operationTypeClient.FindById(GetStrapiToken(), id)
                    .Map(ToView);
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

                return _operationTypeClient.Find(GetStrapiToken(), top, name, active)
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
                var operationTypeHolder = FindByName(view.Name);

                if (operationTypeHolder.IsSome)
                {
                    return $"Operation Type with name: {view.Name} already exists";
                }

                var operationType = new OperationType
                {
                    Name = view.Name,
                    Description = view.Description,
                    Active = true,
                    Inbound = view.Inbound.Equals(1),
                };

                _operationTypeClient.Add(GetStrapiToken(), operationType);

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
                var operationTypeHolder = GetById(view.Id);
                var error = "";                

                operationTypeHolder.Match(some => {
                    
                    var operationType = new OperationType
                    {
                        Id = long.Parse(view.Id),
                        Name = view.Name,
                        Description = view.Description,
                        Inbound = view.Inbound.Equals(1),
                        Active = view.Active.Equals(1)
                    };

                    _logger.LogInformation($"Update operation type: {view.Name}");
                    _operationTypeClient.Update(GetStrapiToken(), operationType);

                }, () => error = $"Operation Type: {view.Name} not found");
                
                if (!String.IsNullOrEmpty(error))
                {
                    return error;
                }

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
                return _operationTypeClient.FindByName(GetStrapiToken(), name)
                    .Map(ToView);
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
                Active = operationType.Active ? 1 : 0,
                Inbound = operationType.Inbound ? 1 : 0
            };
        }
    }
}