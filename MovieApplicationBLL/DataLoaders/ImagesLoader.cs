using MovieApplicationDataBase;
using MovieApplicationDataBase.Movies;
using MovieApplicationDataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApplicationBLL.DataLoaders
{
    public class ImagesLoader
    {
        private ObjectsRepository _objectsRepository;

        private MoviesRepository _moviesRepository;

        public ImagesLoader()
        {
            _objectsRepository = new ObjectsRepository();
            _moviesRepository = new MoviesRepository();
        }
        

    }
}
