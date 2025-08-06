using DataGenerator;
using DataGenerator.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System.Reflection;


var host = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(configuration =>
            {
                configuration.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                configuration.AddJsonFile("appsettings.json", true, true);
                configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("Environment")}.json", true, true);
                configuration.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddLogging(builder => builder.AddSerilog());
                services.Configure<MinioOptions>(context.Configuration.GetSection("S3Connection"));
                services.Configure<OracleOptions>(context.Configuration.GetSection("OracleConnection"));
                services.AddDbContext<ModelContext>((provider, options) => options.UseOracle(provider.GetRequiredService<IOptions<OracleOptions>>().Value.ConnectionString), ServiceLifetime.Singleton);
                services.AddSingleton<App>();
            })
            .Build();

var app = host.Services.GetService<App>();
app?.Run();