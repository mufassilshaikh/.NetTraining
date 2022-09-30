using JWT.Authentication.Server.Core.Contract.Repositories;
using JWT.Authentication.Server.Core.Entities;
using JWT.Authentication.Server.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace JWT.Authentication.Server.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _dbContext;


        public UserRepository(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ValidateUser(string email, string password)
        {
            return await _dbContext.Users.AnyAsync(s => s.Email == email && s.Password == password);
        }

        public async Task<bool> RegisterUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserDetails(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}
