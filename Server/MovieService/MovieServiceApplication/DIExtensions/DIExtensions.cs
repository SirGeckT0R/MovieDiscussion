using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieServiceApplication.Validation;
using System.Reflection;

namespace MovieServiceApplication.Extensions
{
    public static class DIExtensions
    {
        public static void AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
