using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraNavBar;
using MovieApplication.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieApplication
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Form1Presenter presenter;
        private bool isRibbonUpdated = false;

        public Form1()
        {
            InitializeComponent();
        }

        public void RefreshModule()
        {
            ShowModuleMenu(); 
            ShowModuleContent();
        }

        #region UI event Handlers

        private void Form1_Load(object sender, EventArgs e)
        {
            presenter = new Form1Presenter(this);
            InitNavigationBar();
        }

        private void navBarControl1_SelectedLinkChanged(object sender, DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventArgs e)
        {
            var module = e.Link.Item.Tag as BaseModule;
            presenter.ShowModule(module);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            presenter.ShowModule(null);
        }

        private void ribbonControl1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (isRibbonUpdated)
            {
                return;
            }

            presenter.PageChanged(ribbonControl1.SelectedPage);
        }

        private void ribbonControl1_Merge(object sender, DevExpress.XtraBars.Ribbon.RibbonMergeEventArgs e)
        {
            //var ribbon = (sender as RibbonControl);
            //BeginInvoke(new Action(() =>
            //{
            //    if (ribbon.MergedPages.Count > 0)
            //    {
            //        ribbon.SelectedPage = presenter.CurrentModule.ActivePage;
            //    }
            //}));
        }

        #endregion

        #region private Helpers

        private void InitNavigationBar()
        {
            navBarControl1.BeginUpdate();
            var groups = presenter.ApplicationModules.GroupBy(x => x.ModuleGroup);
            foreach (var group in groups)
            {

                var navGroup = navBarControl1.Groups.Add();
                navGroup.Caption = group.Key;
                navGroup.Name = group.Key;
                navGroup.GroupStyle = NavBarGroupStyle.LargeIconsText;
                navGroup.GroupCaptionUseImage = NavBarImage.Small;

                foreach (var module in group)
                {
                    var item = new NavBarItem(module.ModuleName)
                    {
                        LargeImage = module.ModuleImage,
                        Tag = module
                    };
                    navBarControl1.Items.Add(item);
                    navGroup.ItemLinks.Add(item);
                }
            }
            navBarControl1.EndUpdate();
        }

        private void ShowModuleMenu()
        {
            isRibbonUpdated = true;
            ribbonControl1.BeginInit();
            ribbonControl1.UnMergeRibbon();
            if (presenter.CurrentModule != null)
            {
                ribbonControl1.MergeRibbon(presenter.CurrentModule.Menu);
                ribbonControl1.SelectedPage = ribbonControl1.TotalPageCategory.Pages.Cast<RibbonPage>().FirstOrDefault(x => x.Tag == presenter.CurrentModule.ActiveControl);
            }
            ribbonControl1.EndInit();
            isRibbonUpdated = false;
        }

        private void ShowModuleContent()
        {
            panelControl1.Controls.Clear();
            panelControl1.Controls.Add(presenter.CurrentModule);
        }

        #endregion

    }
}
