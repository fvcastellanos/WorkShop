using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using WorkShop.Model;

namespace WorkShop.Repositories
{
    public class ProviderRepository
    {
        private readonly WorkShopContext _dbContext;

        public ProviderRepository(WorkShopContext workShopContext)
        {
            _dbContext = workShopContext;
        }

        public IEnumerable<Provider> FindProviders(int top, string code, string name, int active)
        {
            return _dbContext.Providers.Where(provider => provider.Active.Equals(active) && provider.Code.Contains(code) 
                && provider.Name.Contains(name))
                .Take(top)
                .ToList();            
        }

        public Option<Provider> FindByCode(string code)
        {
            return _dbContext.Providers
                .FirstOrDefault(provider => provider.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));
        }

        public Option<Provider> FindById(string id)
        {
            return _dbContext.Providers.Find(Guid.Parse(id));
        }

        public IEnumerable<Provider> GetActiveProviders()
        {
            return _dbContext.Providers.Where(provider => provider.Active.Equals(1))
                    .ToList();
        }

        public Provider Add(Provider provider)
        {
            _dbContext.Providers.Add(provider);
            _dbContext.SaveChanges();

            return provider;
        }

        public Provider Update(Provider provider)
        {
            _dbContext.Providers.Update(provider);
            _dbContext.SaveChanges();

            return provider;
        }
    }
}