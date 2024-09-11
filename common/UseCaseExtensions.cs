using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace common;

public static class UseCaseExtensions
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection serviceCollection)
    {
        var assembly = Assembly.GetCallingAssembly();

        // Find all interfaces with the UseCaseResolvableAttribute
        var interfacesWithAttribute = assembly.GetTypes()
            .Where(t => t.IsInterface && t.GetCustomAttributes(typeof(UseCaseResolvableAttribute), true).Length > 0)
            .ToList();

        // Find all types with the UseCaseAttribute
        var typesWithUseCaseAttribute = assembly.GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(UseCaseAttribute), true).Length > 0)
            .ToList();

        foreach (var iface in interfacesWithAttribute)
        {
            serviceCollection.AddScoped(iface, provider =>
            {
                var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

                foreach (var type in typesWithUseCaseAttribute)
                {
                    var useCaseAttribute = type.GetCustomAttributes(typeof(UseCaseAttribute), true).First() as UseCaseAttribute;
                    var useCase = UseCaseProvider.GetUseCase(contextAccessor, useCaseAttribute!.PathSegmentIndex);

                    if (useCase == useCaseAttribute.UseCase && iface.IsAssignableFrom(type))
                    {
                        return ActivatorUtilities.CreateInstance(provider, type);
                    }
                }

                return null; // or throw an exception if needed
            });
        }

        return serviceCollection;
    }
}
