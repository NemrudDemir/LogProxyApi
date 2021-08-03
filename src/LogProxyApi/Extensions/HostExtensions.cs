using LogProxyApi.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogProxyApi.Extensions
{
    public static class HostExtensions
    {
        public static IHost InjectSecrets(this IHost host)
        {
            var secretsInjector = host.Services.GetServices<ISecretsInjector>();
            foreach (var injector in secretsInjector)
                injector.InjectSecrets();
            return host;
        }
    }
}
