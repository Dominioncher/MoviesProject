using DevExpress.XtraBars.Ribbon;
using MovieApplication.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieApplication
{
    public class Form1Presenter
    {
        #region Vars for view

        /// <summary>
        /// Список всех модулей приложения
        /// </summary>
        public List<BaseModule> ApplicationModules => ModulesInfo.Instance.Modules;

        /// <summary>
        /// Текущий открытый модуль
        /// </summary>
        public BaseModule CurrentModule
        {
            get => currentModule;
            set
            {
                currentModule = value;
                view.RefreshModule();
            }
        }

        #endregion

        private BaseModule currentModule;

        private Form1 view;

        public Form1Presenter(Form1 form)
        {
            view = form;
            ModulesInfo.Instance.CurrentModuleChanged += Instance_CurrentModuleChanged;
        }

        #region Actions
        public void ShowModule(BaseModule module)
        {
            ModulesInfo.ShowModule(module);
        }
        public void PageChanged(RibbonPage page)
        {
            if (ModulesInfo.Instance.CurrentModule == null)
            {
                return;
            }

            ModulesInfo.Instance.CurrentModule.OnPageChanged(page);
        }
        #endregion

        #region Subscribe application updates
        private void Instance_CurrentModuleChanged(object sender, BaseModule e)
        {
            if (CurrentModule != null)
            {
                CurrentModule.MenuChanged -= CurrentModule_MenuChanged;
            }

            if (e != null)
            {
                e.MenuChanged += CurrentModule_MenuChanged;
            }

            CurrentModule = e;
        }
        private void CurrentModule_MenuChanged(object sender, EventArgs e)
        {
            view.RefreshModule();
        }
        #endregion
    }
}
