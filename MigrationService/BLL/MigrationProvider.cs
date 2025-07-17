using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MigrationService.DB.Adapters;
using MigrationService.Exceptions;

namespace MigrationService.BLL
{
    public class MigrationProvider
    {
        private IDBAdapter _dbAdapter;

        private AppOptions _options;

        private ILogger _logger;

        public MigrationProvider(IDBAdapter dbAdapter, IOptions<AppOptions> options, ILogger<MigrationProvider> logger)
        {
            _dbAdapter = dbAdapter;
            _options = options.Value;
            _logger = logger;
        }

        public void Migrate()
        {
            _logger.LogInformation($"Start migrations");
            _dbAdapter.OpenConnection();

            // Если запускаем в режиме полной очистки БД
            if (_options.Purge)
            {
                _logger.LogInformation("Used purge mode, prepearing for clear DB");
                _dbAdapter.ClearSchema();
                _logger.LogInformation("DB Clear success");

                ClearInstall();
            }
            // Если запускаем в первый раз
            else if (!_dbAdapter.CheckServiceTablesExists())
            {
                ClearInstall();
            }
            // Если запускаем для добавления новых миграций
            else
            {
                InstallAdd();
            }

            _dbAdapter.CloseConnection();
            _logger.LogInformation("All migration sucsessfuly used");
        }


        private void InstallAdd()
        {

            // Оставляем только не зафиксированные миграции
            var head = _dbAdapter.GetMigrationsHead();
            var migrations = GetMigrationsFromDirectory();
            var index = migrations.FindIndex(x =>
            {
                var info = new DirectoryInfo(x);
                return $"{info.Parent.Name}_{info.Name}" == head;
            });
            migrations.RemoveRange(0, index + 1);

            _logger.LogInformation($"Current DB migration: {head}");
            _logger.LogInformation($"Founded {migrations.Count} new migrations");

            // Очищаем лог БД от прошлой не успешной миграции
            _dbAdapter.ClearFailedBeforeMigrate();


            // Применяем новые миграции
            _logger.LogInformation("Stat execute migrations");
            foreach (var migration in migrations)
            {
                ExecuteMigration(migration);
            }

        }

        private void ClearInstall()
        {
            _logger.LogInformation("Create migration tables");
            _dbAdapter.CreateServiceTables();
            var migrations = GetMigrationsFromDirectory();

            _logger.LogInformation($"Founded {migrations.Count} new migrations");
            _logger.LogInformation("Stat execute migrations");
            foreach (var migration in migrations)
            {
                ExecuteMigration(migration);
            }            
        }

        private List<string> GetMigrationsFromDirectory()
        {
            var migrations = Directory.GetFiles(_options.MigrationsPath, "*.sql", SearchOption.AllDirectories).ToList();
            migrations.Sort();
            return migrations;
        }

        private void ExecuteMigration(string path)
        {
            var sql = File.ReadAllText(path);
            var info = new DirectoryInfo(path);
            var migrationName = $"{info.Parent.Name}_{info.Name}";

            try
            {
                _dbAdapter.ExecuteMigration(migrationName, sql);
            }
            catch (DuplicateMigrationException ex)
            {
                _dbAdapter.LogFail($"[duplicate]{migrationName}");
                throw new Exception($"Migration fail {migrationName}", ex);
            }
            catch (Exception ex)
            {
                _dbAdapter.LogFail(migrationName);
                throw new Exception($"Migration fail {migrationName}", ex);
            }
        }
    }
}