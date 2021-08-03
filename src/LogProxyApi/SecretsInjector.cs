using LogProxyApi.Abstractions;
using LogProxyApi.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace LogProxyApi
{
    public class SecretsInjector : ISecretsInjector
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SecretsInjector> _logger;

        public SecretsInjector(IConfiguration configuration, ILogger<SecretsInjector> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void InjectSecrets()
        {
            var airTableApiKey = GetEnvironmentVariable("AIR_TABLE_API_KEY");
            if (string.IsNullOrEmpty(airTableApiKey)) 
                throw new InvalidOperationException($"No {nameof(airTableApiKey)} found!");

            var configSection = $"{nameof(AirTableApiOptions)}:{nameof(AirTableApiOptions.ApiKey)}";
            _configuration[configSection] = airTableApiKey;

            _logger.LogInformation("Injected: {apiKeySection}", configSection);
        }

        private string GetEnvironmentVariable(string key)
        {
            var value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
            if(string.IsNullOrEmpty(value))
                value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(value))
                value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);

            return value;
        }
    }
}
