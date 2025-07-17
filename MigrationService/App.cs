using MigrationService.BLL;

namespace MigrationService
{
    public class App
    {
        MigrationProvider _provider;

        public App(MigrationProvider provider)
        {
            _provider = provider;
        }

        public void Run()
        {
            _provider.Migrate();
        }
    }
}
