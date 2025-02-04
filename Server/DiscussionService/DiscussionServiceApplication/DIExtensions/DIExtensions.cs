using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using DiscussionServiceApplication.Validation;
using System.Reflection;

namespace DiscussionServiceApplication.Extensions
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
