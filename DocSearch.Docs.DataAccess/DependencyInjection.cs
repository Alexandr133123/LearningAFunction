using DocSearch.Docs.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocSearch.Docs.DataAccess
{
    public static class DependencyInjection
    {
        public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DoctorsDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<DoctorsDbContext>();
        }
    }
}
