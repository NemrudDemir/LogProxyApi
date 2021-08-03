using LogProxyApi.Abstractions;
using LogProxyApi.Auth;
using LogProxyApi.Implementations;
using LogProxyApi.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogProxyApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<AirTableApiOptions>(Configuration.GetSection(nameof(AirTableApiOptions)));
            services.Configure<AuthorizationOptions>(Configuration.GetSection(nameof(AuthorizationOptions)));

            services.AddTransient<ISecretsInjector, SecretsInjector>();

            services.AddTransient<IThirdPartyApiCommunicator, AirTableApiCommunicator>();
            services.AddTransient<IIdGenerator, SampleIdGenerator>();
            services.AddTransient<IAirTableRestClient, AirTableRestClient>();

            services.AddAutoMapper(typeof(Startup));

            const string authScheme = "BasicAuthentication";
            services.AddAuthentication(authScheme)
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(authScheme, null);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
