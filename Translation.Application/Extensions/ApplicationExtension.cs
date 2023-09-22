using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Translation.Application.Commands;
using Translation.Application.Queries;

namespace Translation.Application.Extensions
{
    public static class ApplicationExtension
    {
        public static void AddApplicationComponents(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Application CQ's
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddSingleton<ReadTranslationQuery>();
            // Register Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
