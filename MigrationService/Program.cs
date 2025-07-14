using MigrationService;
using MigrationService.DBAdapters;
using Oracle.ManagedDataAccess.Client;
using System.Transactions;

var envs = Environment.GetEnvironmentVariables();

//var migrationsPath = envs["path"].ToString();
//var user = envs["user"].ToString();
//var password = envs["password"].ToString();
//var host = envs["host"].ToString();


var builder = new OracleConnectionStringBuilder();
builder.UserID = "video_rent";
builder.Password = "video_rent";
builder.DataSource = "localhost:1521/XEPDB1";
var connectionString = builder.ConnectionString;



var oracleAdapter = new OracleAdapter(new OracleConnection(connectionString));



var provider = new MigrationProvider(oracleAdapter);
provider.Prune = true;
provider.MigrationPath = "C:\\Users\\kushn\\source\\repos\\MovieApplication\\Migrations";


provider.Migrate();