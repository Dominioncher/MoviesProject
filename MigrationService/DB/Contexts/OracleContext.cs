using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;

namespace MigrationService.DB.Contexts
{
    internal class OracleContext : IDBContext
    {
        AppOptions _options;

        public OracleContext(IOptions<AppOptions> options)
        {
            _options = options.Value;
        }

        public string GetConnectionString()
        {
            var builder = new OracleConnectionStringBuilder();
            builder.UserID = _options.User;
            builder.Password = _options.Password;
            builder.DataSource = _options.Host;
            return builder.ConnectionString;
        }
    }
}
