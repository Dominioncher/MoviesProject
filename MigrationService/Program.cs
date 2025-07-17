using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MigrationService;
using MigrationService.BLL;
using MigrationService.DB.Adapters;
using MigrationService.DB.Contexts;
using Serilog;
using System.Reflection;

var host = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(configuration =>
            {
                configuration.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                configuration.AddJsonFile("appsettings.json", false, false);
                configuration.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<AppOptions>(context.Configuration);
                services.AddSingleton<App>();
                services.AddTransient<MigrationProvider>();
                services.AddLogging(builder => builder.AddSerilog());

                var adapter = context.Configuration.GetValue<string>("DBProvider");
                switch (adapter)
                {
                    case "Oracle":
                        services.AddTransient<IDBContext, OracleContext>();
                        services.AddTransient<IDBAdapter, OracleAdapter>();
                        break;
                    default:
                        break;
                }
            })
            .Build();

var app = host.Services.GetService<App>();
app?.Run();