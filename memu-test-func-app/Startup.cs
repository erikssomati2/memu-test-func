using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

[assembly: FunctionsStartup(typeof(memu_test_func_app.Startup))]

namespace memu_test_func_app
{
    class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {

            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
            configBuilder.AddEnvironmentVariables();

            var config = configBuilder.Build();

            if (config["Environment"] != "Local")
            {
                string cs = config["ConnectionString"];
                builder.ConfigurationBuilder.AddAzureAppConfiguration(opt =>
                {
                    opt.Connect(cs)
                        .Select(KeyFilter.Any, LabelFilter.Null)
                        .Select(KeyFilter.Any, config["Environment"]);
                });
            }
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
        }
    }
}
