using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using WorkShop.Model;

namespace WorkShop.Providers
{
    public class TokenProvider
    {
        private readonly WorkShopContext _dbContext;

        public TokenProvider(WorkShopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void StoreToken(string user, string value) {

            var existingUserToken = _dbContext.UserTokens.Where(userToken => userToken.User.Equals(user))
                .FirstOrDefault();

            if (existingUserToken != null)
            {
                existingUserToken.Token = value;
                _dbContext.UserTokens.Update(existingUserToken);
                _dbContext.SaveChanges();
                return;
            }

            var userToken = new UserToken
            {
                User = user,
                Token = value
            };

            _dbContext.UserTokens.Add(userToken);
            _dbContext.SaveChanges();
        }

        public string GetToken(string user)
        {
            return _dbContext.UserTokens
                .Where(ut => ut.User.Equals(user))
                .Select(ut => ut.Token)
                .FirstOrDefault();
        }

        public void RemoveToken(string user)
        {
            var userToken = _dbContext.UserTokens
                .Where(ut => ut.User.Equals(user))
                .FirstOrDefault();

            if (userToken != null)
            {
                _dbContext.UserTokens.Remove(userToken);
                _dbContext.SaveChanges();
            }
        }
    }
}