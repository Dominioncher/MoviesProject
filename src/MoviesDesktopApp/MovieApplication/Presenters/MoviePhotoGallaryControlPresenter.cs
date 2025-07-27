using DevExpress.XtraSplashScreen;
using MovieApplication.Controls;
using MovieApplication.DTO;
using MovieApplication.UserControls;
using MovieApplicationDataBase.Movies;
using MovieApplicationDataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieApplication.Modules
{
    public class MoviePhotoGallaryControlPresenter
    {
        public List<Photo> Images { get; private set; } = new List<Photo>();

        private MoviesRepository _moviesRepository;

        private ObjectsRepository _objectsRepository;

        private MoviePhotoGalleryControl _view;

        public MoviePhotoGallaryControlPresenter(MoviePhotoGalleryControl view)
        {
            _view = view;
            _moviesRepository = new MoviesRepository();
            _objectsRepository = new ObjectsRepository();
        }

        #region Actions

        public void UploadMovieImages()
        {
            if(_view.MovieID == null)
            {
                return;
            }

            // Choose Images
            string[] paths;
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Images |*.png;*.bmp;*.dib;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.tif;*.tiff;*.ico";
                openFileDialog.Multiselect = true;
                var result = openFileDialog.ShowDialog();
                if (result != DialogResult.OK)
                {
                    return;
                }

                paths = openFileDialog.FileNames;
            }

            // Upload tmp images to DB
            var guids = new List<Guid>();
            foreach (var path in paths)
            {
                var image = Image.FromFile(path);
                var guid = _objectsRepository.AddObject(image);
                guids.Add(guid);
                image.Dispose();
                GC.Collect();
            }

            // Add Movies Links
            _moviesRepository.AddMovieImages((int)_view.MovieID, guids);

            // Reload Images
            LoadImages();
        }

        public void LoadImages()
        {
            Images = new List<Photo>();
            if (_view.MovieID == null)
            {
                return;
            }

            var ids = _moviesRepository.GetMovieImages((int)_view.MovieID);
            foreach (var id in ids)
            {
                Images.Add(new Photo()
                {
                    ID = id,
                    Image = (Image)_objectsRepository.GetObject(id)
                });
            }

            _view.RefreshGrid();
        }

        #endregion
    }
}
