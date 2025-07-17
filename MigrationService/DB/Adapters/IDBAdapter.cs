namespace MigrationService.DB.Adapters
{
    public interface IDBAdapter
    {
        public void OpenConnection();

        public void CloseConnection();

        public void ClearSchema();

        public void CreateServiceTables();

        public bool CheckServiceTablesExists();

        public void ClearFailedBeforeMigrate();

        public string GetMigrationsHead();

        public void ExecuteMigration(string migrationName, string sql);

        public void LogFail(string migrationName);
    }
}
