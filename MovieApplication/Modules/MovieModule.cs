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
using MovieApplication.Properties;
using MovieApplicationResources;
using DevExpress.Utils;
using DevExpress.XtraBars.Ribbon;

namespace MovieApplication.Modules
{
	public partial class MovieModule: BaseModule
	{
        #region Public vars
        public override string ModuleName => "Movies";

		public override Image ModuleImage => ImagesHelper.GetImage("Movie", ImagesHelper.ImageSize.Large);

		public override string ModuleGroup => "Catalog";
        #endregion

        private MovieModulePresenter _presenter; 

        public MovieModule()
		{
            InitializeComponent();
        }

        #region Base overrides
        public override void OnInit()
        {
            _presenter = new MovieModulePresenter(this);
        }

        public override void OnInitData()
        {
            base.OnInitData();
            _presenter.LoadMovies();
            gridControl1.DataSource = _presenter.Movies;
        }
        #endregion

        #region UI events Handlers
        private void advBandedGridView1_DoubleClick(object sender, EventArgs e)
        {
            var args = e as DXMouseEventArgs;
            var hit_info = advBandedGridView1.CalcHitInfo(args.Location);
            if (hit_info.InDataRow)
            {
                _presenter.ShowEditForm(advBandedGridView1.FocusedRowObject);
            }
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _presenter.ShowEditForm(advBandedGridView1.FocusedRowObject);
        }
        #endregion

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _presenter.ShowAddForm();
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _presenter.RemoveMovie(advBandedGridView1.FocusedRowObject);
        }
    }
}
