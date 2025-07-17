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
        public bool Purge { get; set; } = false;

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
            if (Purge)
            {
                _dbAdapter.ClearSchema();
                ClearInstall();
                return;
            }

            // Если запускаем в первый раз
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

            // Оставляем только не зафиксированные миграции
            var head = _dbAdapter.GetMigrationsHead();
            var migrations = GetMigrationsFromDirectory();
            var index = migrations.FindIndex(x =>
            {
                var info = new DirectoryInfo(x);
                return $"{info.Parent.Name}_{info.Name}" == head;
            });
            migrations.RemoveRange(0, index + 1);

            // Очищаем лог БД от прошлой не успешной миграции
            _dbAdapter.ClearFailedBeforeMigrate();

            // Применяем новые миграции
            foreach (var migration in migrations)
            {
                ExecuteMigration(migration);
            }

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