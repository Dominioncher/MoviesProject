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

namespace MovieApplication.UserControls
{
	public partial class BaseDetailsControl: DevExpress.XtraEditors.XtraUserControl
	{
        public event EventHandler OnClosed;

        public RibbonControl Menu => ribbonControl1;

        protected string DetailsPageText
        {
            get => ribbonPage2.Text;
            set { ribbonPage2.Text = value; }
        }

        public BaseDetailsControl()
		{
            InitializeComponent();
		}

		public virtual void InitData()
		{

		}

		public virtual void Save()
		{

		}

        public virtual void Close()
        {
            OnClosed?.Invoke(this, EventArgs.Empty);
            Dispose();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
            Close();
        }
    }
}
