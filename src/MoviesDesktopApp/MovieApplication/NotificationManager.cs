using DevExpress.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApplication
{
    public class NotificationManager
    {
        public event EventHandler<string> OnNotify;

        public static NotificationManager Instance { get; private set; }

        static NotificationManager()
        {
            Instance = new NotificationManager();
        }

        public void Notify(string message)
        {
            OnNotify?.Invoke(this, message);
        }

        public void ExecuteWithErrorHandling(Action action, string failMessage)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                Notify(failMessage);
            }
        }
    }
}
