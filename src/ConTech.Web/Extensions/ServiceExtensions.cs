using ConTech.Core;
using ConTech.Core.Features.Level;
using ConTech.Core.Features.Project;
using ConTech.Core.Features.View;
using ConTech.Data.DatabaseSpecific;
namespace ConTech.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment env)
        {
            /* settings */
            services.AddOptions<SystemOptions>()
                .Configure(options => configuration.GetSection("System").Bind(options))
                .Validate(x => x.Validate());

            /* Registrations */
            //services.AddTransient(x => new DataAccessAdapter());
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IProjectViewRepository, ProjectViewRepository>();
            services.AddTransient<IViewLevelRepository, ViewLevelRepository>();

            return services;
        }
    }
}
