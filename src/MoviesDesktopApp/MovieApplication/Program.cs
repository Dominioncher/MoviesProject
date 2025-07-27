using DevExpress.XtraSplashScreen;
using MovieApplication.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieApplication
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show Splash Screen
            SplashScreenManager.ShowForm(typeof(SplashScreen));

            // Init Application
            RegisterModules();
            InitModules();
            var form = new Form1();

            // End Splash
            SplashScreenManager.CloseForm();

            Application.Run(form);
        }

        #region Helpers
        private static void RegisterModules()
        {
            ModulesInfo.RegisterModule<MovieModule>();
            ModulesInfo.RegisterModule<RentalsModule>();
            ModulesInfo.RegisterModule<TopMoviesModule>();
        }
        private static void InitModules()
        {
            ModulesInfo.InitModules();
        }
        #endregion
    }
}
