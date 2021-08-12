using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using WorkShop.Model;

namespace WorkShop.Repositories
{
    public class OperationTypeRepository
    {
        private readonly WorkShopContext _dbContext;

        public OperationTypeRepository(WorkShopContext workShopContext)
        {
            _dbContext = workShopContext;
        }

        public IEnumerable<OperationType> FindOperationTypes(int top, string name, int active)
        {
            return _dbContext.OperationTypes.Where(operationType => operationType.Active.Equals(active) && 
                operationType.Name.Contains(name))
                .Take(top)
                .ToList();
        }

        public Option<OperationType> FindById(string id)
        {
            return _dbContext.OperationTypes.Find(Guid.Parse(id));
        }

        public Option<OperationType> FindByName(string name)
        {
            return _dbContext.OperationTypes.FirstOrDefault(operationType => 
                operationType.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public OperationType Add(OperationType operationType)
        {
            _dbContext.OperationTypes.Add(operationType);
            _dbContext.SaveChanges();

            return operationType;
        }

        public OperationType Update(OperationType operationType)
        {
            _dbContext.OperationTypes.Update(operationType);
            _dbContext.SaveChanges();

            return operationType;
        }

    }
}