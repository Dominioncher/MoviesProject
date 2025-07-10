using DevExpress.Utils.Controls;
using DevExpress.XtraEditors;
using MovieApplicationDataBase;
using MovieApplicationDataBase.Movies;
using MovieApplicationResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MovieApplication.DataSet1;

namespace MovieApplication.Modules
{
	public partial class TopMoviesModule: BaseModule
	{

        public override string ModuleName => "Top";

        public override Image ModuleImage => ImagesHelper.GetImage("Movie", ImagesHelper.ImageSize.Large);

        public override string ModuleGroup => "Catalog";

        private MoviesRepository _repository;

        public TopMoviesModule()
        {
            InitializeComponent();
        }

        public override void OnInit()
        {
            _repository = new MoviesRepository();
        }

        public override void OnInitData()
        {
            base.OnInitData();
            tOP_10_MOVIES_VIEWTableAdapter.Fill(dataSet1.TOP_10_MOVIES_VIEW);          
        }

        private void tileView1_ItemCustomize(object sender, DevExpress.XtraGrid.Views.Tile.TileViewItemCustomizeEventArgs e)
        {
            var row = tileView1.GetDataRow(e.RowHandle) as TOP_10_MOVIES_VIEWRow;
            if (row.IsPHOTONull())
            {
                return;
            }
            
            e.Item.BackgroundImage = ImageHelpers.FromByteArray(row.PHOTO);
        }
    }
}
