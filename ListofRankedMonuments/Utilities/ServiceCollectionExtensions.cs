using System.Reflection;

namespace QUANLYVANHOA.Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositoriesAndServices(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsSubclassOf(typeof(Attribute)))  // Loại trừ các lớp Attribute
                .Select(t => new
                {
                    Implementation = t,
                    Interfaces = t.GetInterfaces()
                })
                .Where(t => t.Interfaces != null && t.Interfaces.Any());

            foreach (var type in types)
            {
                foreach (var @interface in type.Interfaces)
                {
                    // Kiểm tra lại để loại trừ các interface không hợp lệ
                    if (!IsValidForRegistration(@interface, type.Implementation))
                    {
                        continue;
                    }

                    // Đăng ký các dịch vụ hợp lệ
                    services.AddScoped(@interface, type.Implementation);
                }
            }

            foreach (var type in types)
            {
                foreach (var @interface in type.Interfaces)
                {
                    Console.WriteLine($"Registering {@interface.Name} with {type.Implementation.Name}"); // Ghi log ra terminal
                    services.AddScoped(@interface, type.Implementation);
                }
            }

        }

        private static bool IsValidForRegistration(Type @interface, Type implementation)
        {
            // Loại trừ các interface không hợp lệ
            if (@interface == typeof(int) || implementation == typeof(int) || implementation.IsSubclassOf(typeof(Attribute)))
            {
                return false;
            }
            return true;
        }
    }
}
