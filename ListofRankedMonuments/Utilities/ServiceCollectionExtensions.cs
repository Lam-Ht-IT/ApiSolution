using System.Reflection;

namespace QUANLYVANHOA.Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositoriesAndServices(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Implementation = t,
                    Interface = t.GetInterfaces().FirstOrDefault()
                })
                .Where(t => t.Interface != null);

            foreach (var type in types)
            {
                services.AddScoped(type.Interface, type.Implementation);
            }
        }
    }
}
