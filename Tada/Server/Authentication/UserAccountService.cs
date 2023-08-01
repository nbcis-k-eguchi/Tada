using Tada.Server.Database;
using Tada.Shared;

namespace Tada.Server.Authentication
{
    public class UserAccountService
    {
        private readonly List<AccountUser> _userAccounts = new List<AccountUser>();
        private UserSession _userSession = new UserSession();

        public UserAccountService()
        {
            var dbContext = new AccountUserDbContext();
            _userAccounts = dbContext.FindAll();
        }

        public AccountUser? FindUser(string email, string password)
        {
            return _userAccounts.FirstOrDefault(x => x.Email == email && x.Password == password);
        }

        public AccountUser? FindUser(int id)
        {
            return _userAccounts.FirstOrDefault(x => x.Id == id);
        }

        public bool UpdateToken(AccountUser user)
        {
            var dbContext = new AccountUserDbContext();
            return dbContext.Update(user);
        }

    }
}
