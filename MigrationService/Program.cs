using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MigrationService;
using MigrationService.DBAdapters;
using Oracle.ManagedDataAccess.Client;
using System.Reflection;
using System.Transactions;


IConfigurationBuilder CreateConfigurationBuilder(string[] args)
{
    return new ConfigurationBuilder()
        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
        .AddJsonFile("appsettings.json", false, false)
        .AddEnvironmentVariables();
}




var configuration = CreateConfigurationBuilder(args).Build();
var services = new ServiceCollection().BuildServiceProvider();



void ConfigureServices(IServiceCollection services, Ico)
{
    
}


void Run()
{

}



var builder = new OracleConnectionStringBuilder();
builder.UserID = "video_rent";
builder.Password = "video_rent";
builder.DataSource = "localhost:1521/XEPDB1";
var connectionString = builder.ConnectionString;



var oracleAdapter = new OracleAdapter(new OracleConnection(connectionString));



var provider = new MigrationProvider(oracleAdapter);
provider.Purge = true;
provider.MigrationPath = "C:\\Users\\kushn\\source\\repos\\MovieApplication\\Migrations";


provider.Migrate();

