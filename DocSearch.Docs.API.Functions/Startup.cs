using DocSearch.Docs.API.Functions;
using DocSearch.Docs.DataAccess;
using DocSearch.Docs.DataAccess.Data;
using DocSearch.Docs.DataAccess.Mutations;
using DocSearch.Docs.DataAccess.Queries;
using HotChocolate.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace DocSearch.Docs.API.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            builder.Services.AddDataAccessServices(configuration);

            builder.AddGraphQLFunction(b =>
            {
                b.AddQueryType<DoctorsQuery>()
                    .RegisterDbContext<DoctorsDbContext>(DbContextKind.Synchronized)
                    .AddProjections()
                    .AddFiltering()
                    .AddSorting();

                b.AddMutationConventions();

                b.AddMutationType<DoctorsMutation>()
                    .RegisterDbContext<DoctorsDbContext>(DbContextKind.Synchronized);
            });
        }
    }
}
