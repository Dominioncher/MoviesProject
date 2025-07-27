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
using MovieApplicationDataBase.Movies;
using MovieApplication.Modules;

namespace MovieApplication.UserControls
{
	public partial class MovieDetailsControl: BaseDetailsControl
	{
		public DBMovie Movie { get; set; }

        private MovieDetailsPresenter _presenter;

        public MovieDetailsControl()
		{
            InitializeComponent();  
		}

        public override void InitData()
        {
            base.InitData();
            _presenter = new MovieDetailsPresenter(this);            
            EditorsHelper.FillJanresCheckedComboBox(editGenre.Properties);

            if (Movie == null)
            {
                DetailsPageText = "New film";
                return;
            }

            editTitle.EditValue = Movie.Title;
            editDescription.EditValue = Movie.Description;
            editImage.EditValue = Movie.Image;
            editReleaseDate.EditValue = Movie.ReleaseDate;
            editGenre.EditValue = Movie.Ganres;
            DetailsPageText = Movie.Title;
        }

        private void editTitle_EditValueChanged(object sender, EventArgs e)
        {
        }

        public override void Save()
        {
            base.Save();

            _presenter.Movie.Title = editTitle.EditValue as string;
            _presenter.Movie.Description = editDescription.EditValue as string;
            _presenter.Movie.Image = editImage.EditValue as Image;
            _presenter.Movie.ReleaseDate = editReleaseDate.EditValue as DateTime?;
            _presenter.Movie.Ganres = editGenre.EditValue as List<DBGanres>;
            _presenter.UpdateMovie();
            DetailsPageText = _presenter.Movie.Title;
        }

        private void editRaiting_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
