using DevExpress.XtraEditors;
using MovieApplication.Modules;
using MovieApplicationDataBase;
using MovieApplicationDataBase.Movies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieApplication.Controls
{
    public partial class MoviePhotoGalleryControl : DevExpress.XtraEditors.XtraUserControl
    {
        public int? MovieID
        {
            get => movieID; 
            set
            {
                movieID = value; 
                presenter.LoadImages();
            }
        }

        private int? movieID;

        private MoviePhotoGallaryControlPresenter presenter { get; set; }

        public MoviePhotoGalleryControl()
        {
            InitializeComponent();
            movieID = null;
            presenter = new MoviePhotoGallaryControlPresenter(this);
        }

        public void RefreshGrid()
        {
            gridControl1.DataSource = presenter.Images;
        }

        private void MoviePhotoGalleryControl_Load(object sender, EventArgs e)
        {
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            presenter.UploadMovieImages();
        }
    }
}
