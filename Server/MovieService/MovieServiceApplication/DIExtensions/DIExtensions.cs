using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieServiceApplication.Behaviors;
using MovieServiceApplication.Interfaces.UseCases.Helpers;
using MovieServiceApplication.UseCases.Movies.Helpers;
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
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public static void AddHelpers(this IServiceCollection services)
        {
            services.AddScoped<IDetailedMovieHelper, DetailedMovieHelper>();
        }
    }
}
