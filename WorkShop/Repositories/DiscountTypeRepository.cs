using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using WorkShop.Model;

namespace WorkShop.Repositories
{
    public class DiscountTypeRepository
    {
        private readonly WorkShopContext _dbContext;

        public DiscountTypeRepository(WorkShopContext workShopContext)
        {
            _dbContext = workShopContext;
        }

        public IEnumerable<DiscountType> FindDiscountTypes(int top, string name, int active)
        {
            return _dbContext.DiscountTypes.Where(discountType => discountType.Active.Equals(active) && 
                discountType.Name.Contains(name))
                .Take(top)
                .ToList();                        
        }

        public Option<DiscountType> FindByName(string name)
        {
            return _dbContext.DiscountTypes.FirstOrDefault(discountType => 
                    discountType.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));            
        }

        public Option<DiscountType> FindById(string id)
        {
            return _dbContext.DiscountTypes.Find(Guid.Parse(id));            
        }

        public DiscountType Add(DiscountType discountType)
        {
            _dbContext.DiscountTypes.Add(discountType);
            _dbContext.SaveChanges();

            return discountType;
        }

        public DiscountType Update(DiscountType discountType)
        {
            _dbContext.DiscountTypes.Update(discountType);
            _dbContext.SaveChanges();

            return discountType;
        }
    }
}