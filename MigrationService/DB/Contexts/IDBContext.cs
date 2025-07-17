using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationService.DB.Contexts
{
    public interface IDBContext
    {
        public string GetConnectionString();
    }
}
