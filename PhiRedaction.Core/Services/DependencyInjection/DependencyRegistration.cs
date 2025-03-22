using Microsoft.Extensions.DependencyInjection;
using PhiRedaction.Core.Services.Redaction;
using PhiRedaction.Core.Services.Redaction.LabOrder;

namespace PhiRedaction.Core.Services.DependencyInjection
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IRedactionService, LabOrderRedactionService>();
            return services;
        }
    }
}
