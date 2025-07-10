using DevExpress.Data;
using DevExpress.XtraSplashScreen;
using MovieApplication.UserControls;
using MovieApplicationDataBase.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApplication.Modules
{
    public class MovieModulePresenter
    {
        public VirtualServerModeSource Movies { get; private set; } = new VirtualServerModeSource();

        private MoviesRepository _repository;

        private MovieModule _view;

        public MovieModulePresenter(MovieModule view)
        {
            _view = view;
            _repository = new MoviesRepository();
        }

        #region Actions

        public void LoadMovies()
        {
            //Movies = _repository.GetAllMovies();
            //var topMovies = _repository.GetTop5Movies();

            Movies = new VirtualServerModeSource
            {
                RowType = typeof(DBMovie)
            };
            Movies.ConfigurationChanged += Source_ConfigurationChanged;
            Movies.MoreRows += Source_MoreRows;
        }

        private void Source_MoreRows(object sender, VirtualServerModeRowsEventArgs e)
        {
            e.RowsTask = Task.Factory.StartNew(() =>
            {
                return new VirtualServerModeRowsTaskResult(GetMovies(e.CurrentRowCount), true, e.UserData);
            },
            e.CancellationToken);
        }

        private void Source_ConfigurationChanged(object sender, VirtualServerModeRowsEventArgs e)
        {
            e.UserData = GetMovies(0);
        }

        private List<DBMovie> GetMovies(int index)
        {
            return _repository.GetAllMovies();
        }

        public void ShowEditForm(object movie)
        {
            var details = new MovieDetailsControl();
            details.Movie = movie as DBMovie;
            _repository.IncreaceMovieView((int)details.Movie.ID);
            _view.OpenDetailView(details);
        }

        public void ShowAddForm()
        {
            var details = new MovieDetailsControl();
            _view.OpenDetailView(details);
        }

        public void RemoveMovie(object movie)
        {
            var id = (int)(movie as DBMovie).ID;
            _repository.RemoveMovie(id);
        }

        #endregion
    }
}
