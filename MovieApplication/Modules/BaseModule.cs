using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using MovieApplication.UserControls;
using DevExpress.Utils.Extensions;

namespace MovieApplication.Modules
{
	public partial class BaseModule: DevExpress.XtraEditors.XtraUserControl
	{
		public event EventHandler MenuChanged;

		public RibbonControl Menu => ribbonControl1;

		public virtual string ModuleName => "Module";

		public virtual Image ModuleImage => null;

		public virtual string ModuleGroup => "Default Group";

        public new Control ActiveControl { get; set; }

		private BaseDetailsControl DetailsControl;

		private List<Control> DefaultControls;

        public BaseModule()
		{
            InitializeComponent();
            ActiveControl = this;
		}

		public virtual void OnInit()
		{
        }

		public virtual void OnInitData()
        {
        }

		public virtual void OnClosed()
		{

		}

		public virtual void OnPageChanged(RibbonPage page)
		{
			if (page.Tag == null)
			{
				return;
			}

			if (page.Tag == this)
			{
				ShowModule();
            }

			if (page.Tag == DetailsControl)
			{
				ShowDetails();
			}
        }

		public void OpenDetailView(BaseDetailsControl detailsControl)
		{
			// Close previous control
            if (DetailsControl != null)
            {
                DetailsControl.OnClosed -= DetailsControl_OnClosed;
                DetailsControl.Close();
                Menu.UnMergeRibbon();
            }

            // Init
            DetailsControl = detailsControl;
            DetailsControl.OnClosed += DetailsControl_OnClosed;
            DetailsControl.InitData();
            ActiveControl = DetailsControl;

            // Hide controls
            HideAllControls();

            // Show detail
            detailsControl.Dock = DockStyle.Fill;
            Controls.Add(detailsControl);

            // Add menu
            detailsControl.Menu.TotalPageCategory.Pages.Cast<RibbonPage>().ForEach(x => x.Tag = detailsControl);
			Menu.MergeRibbon(detailsControl.Menu);
            MenuChanged?.Invoke(this, EventArgs.Empty);
		}

        private void DetailsControl_OnClosed(object sender, EventArgs e)
        {
            DetailsControl.OnClosed -= DetailsControl_OnClosed;
			ShowModule();
            Menu.UnMergeRibbon();
            MenuChanged?.Invoke(this, EventArgs.Empty);
        }

		private void HideAllControls()
		{
            Controls.Cast<Control>().ForEach(x => x.Hide());
        }

		private void ShowModule()
		{
            OnInitData();
            HideAllControls();
            ActiveControl = this;
            DefaultControls.ForEach(x => x.Show());
		}

		private void ShowDetails()
        {
            HideAllControls();
            ActiveControl = DetailsControl;
            DetailsControl.Show();
		}

        private void ribbonControl1_SelectedPageChanged(object sender, EventArgs e)
        {

        }

        private void BaseModule_Load(object sender, EventArgs e)
        {
            DefaultControls = Controls.Cast<Control>().ToList();
        }
    }
}
