using JWT.Authentication.Server.Core.Contract.Repositories;
using JWT.Authentication.Server.Infrastructure.Contexts;
using JWT.Authentication.Server.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;

namespace JWT.Authentication.Server.Infrastructure.Extensions
{
    public static class ServiceCollectionExtenions
    {
        public static void AddConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Register Repository
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion

            #region Database
            services.AddScoped<IdentityDbContext>()
                     .AddDbContextPool<IdentityDbContext>(options =>
                     {
                         options.UseSqlServer(configuration.GetConnectionString("IdentityDbContext"));
                     });
            #endregion
        }
    }
}
