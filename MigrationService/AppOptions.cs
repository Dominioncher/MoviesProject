using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationService
{
    public class AppOptions
    {
        public bool Purge { get; set; }

        public string MigrationsPath { get; set; }

        public string DBProvider { get; set; }

        public string Host { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}
