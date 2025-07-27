using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
using MovieApplication.Modules;
using MovieApplication.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieApplication
{
    public class ModulesInfo
    {
        public static ModulesInfo Instance => instance;

        public event EventHandler<BaseModule> CurrentModuleChanged;

        public BaseModule CurrentModule
        {
            get => currentModule;
            set
            {
                currentModule = value;
                CurrentModuleChanged?.Invoke(Instance, CurrentModule);
            }
        }

        public List<BaseModule> Modules { get; set; } = new List<BaseModule>();

        public List<Type> RegisteredModulesTypes { get; set; } = new List<Type>();

        private BaseModule currentModule;

        private static ModulesInfo instance;

        static ModulesInfo()
        {
            instance = new ModulesInfo();
        }

        public static void RegisterModule<T>() where T: BaseModule
        {
            Instance.RegisteredModulesTypes.Add(typeof(T));
        }

        public static void InitModules()
        {
            foreach (var moduleType in Instance.RegisteredModulesTypes)
            {
                var module = Activator.CreateInstance(moduleType) as BaseModule;
                module.Dock = System.Windows.Forms.DockStyle.Fill;
                foreach (RibbonPage page in module.Menu.TotalPageCategory.Pages)
                {
                    page.Tag = module;
                }
                Instance.Modules.Add(module);
                module.OnInit();
            }
        }

        public static void ShowModule(BaseModule module)
        {
            if (module != null)
            {
                module.OnInitData();
            }

            Instance.CurrentModule = module;
        }
    }
}
