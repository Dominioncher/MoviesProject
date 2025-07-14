using MigrationService.DBAdapters;
using MigrationService.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationService
{
    public class MigrationProvider
    {
        public bool Prune { get; set; } = false;

        public string MigrationPath { get; set; }

        private OracleAdapter _dbAdapter;

        public MigrationProvider(OracleAdapter dbAdapter)
        {
            _dbAdapter = dbAdapter;
        }

        public void Migrate()
        {
            _dbAdapter.OpenConnection();

            // Если запускаем в режиме полной очистки БД
            if (Prune)
            {
                _dbAdapter.ClearSchema();
                ClearInstall();
                return;
            }

            // Если запускаем в 1 раз
            var firstInstall = !_dbAdapter.CheckServiceTablesExists();
            if (firstInstall)
            {
                ClearInstall();
                return;
            }

            // Если запускаем для добавления новых миграций
            InstallAdd();

            _dbAdapter.CloseConnection();
        }

        private void InstallAdd()
        {
            var lastMigrationName = _dbAdapter.GetMigrationsHead();
            var migrations = GetMigrationsFromDirectory();

        }

        private void ClearInstall()
        {
            _dbAdapter.CreateServiceTables();
            var migrations = GetMigrationsFromDirectory();
            foreach (var migration in migrations)
            {
                ExecuteMigration(migration);
            }            
        }

        private List<string> GetMigrationsFromDirectory()
        {
            var migrations = Directory.GetFiles(MigrationPath, "*.sql", SearchOption.AllDirectories).ToList();
            migrations.Sort();
            return migrations;
        }

        private void ExecuteMigration(string path)
        {
            var quieries = File.ReadAllText(path).Split(";", StringSplitOptions.RemoveEmptyEntries);
            var info = new DirectoryInfo(path);
            var migrationName = $"{info.Parent.Name}_{info.Name}";

            try
            {
                _dbAdapter.StartMigration(migrationName);
                _dbAdapter.ExecuteMigrationQuieries(quieries);
                _dbAdapter.MigrationSuccess(migrationName);
            }
            catch (DuplicateMigrationException)
            {
                _dbAdapter.MigrationFail($"[duplicate]{migrationName}");
                throw;
            }
            catch (Exception ex)
            {
                _dbAdapter.MigrationFail(migrationName);
                throw new Exception($"Migration fail {migrationName}", ex);
            }
        }
    }
}
