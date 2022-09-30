using JWT.Authentication.Server.Core.Entities;

namespace JWT.Authentication.Server.Core.Contract.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> ValidateUser(string email, string password);
        public Task<bool> RegisterUser(User user);
        Task<User> GetUserDetails(string email);

    }
}
