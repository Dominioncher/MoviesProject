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
    public class MovieDetailsPresenter
    {
        public DBMovie Movie { get; private set; } = new DBMovie();

        private MoviesRepository _repository;

        private MovieDetailsControl _view;

        public MovieDetailsPresenter(MovieDetailsControl view)
        {
            _view = view;
            _repository = new MoviesRepository();
        }

        #region Actions

        public void UpdateMovie()
        {
            if (Movie.ID == null)
            {
                Movie.ID = _repository.CreateMovie(Movie.Title);
            }

            _repository.UpdateMovie(Movie);
        }

        #endregion
    }
}
