using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationService.Exceptions
{
    internal class DuplicateMigrationException : Exception
    {
        public DuplicateMigrationException(string? message = null, Exception? innerException = null)
            : base(message, innerException)
        {

        }
    }
}
