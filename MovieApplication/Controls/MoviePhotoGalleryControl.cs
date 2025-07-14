using DevExpress.XtraEditors;
using MovieApplicationDataBase.Movies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieApplication.Controls
{
    public partial class MoviePhotoGalleryControl : DevExpress.XtraEditors.XtraUserControl
    {
        public int MovieID { get; set; }

        private MoviesRepository _moviesRepository { get; set; }

        public MoviePhotoGalleryControl()
        {
            InitializeComponent();
            var _moviesRepository = new MoviesRepository();
        }
        
        private void LoadImages()
        {
            
        }
    }
}
