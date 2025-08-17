using AIDataGen.Core;
using DataGenerator.DB;
using DataGenerator.SyntClasses;
using GenAI;
using Newtonsoft.Json;

namespace DataGenerator
{
    public class App
    {
        private ModelContext _context;

        private Generator _generator;

        public App(ModelContext context, Generator generator)
        {
            _context = context;
            _generator = generator;
        }

        public void Run()
        {
            var moviesTask = UploadMovies();
            moviesTask.Wait();
        }

        private async Task UploadMovies()
        {
            var movies = _generator.Generate<SyntMovie>(10, "");
            await foreach (var movie in movies)
            {
                var rewiews = _generator.Generate<SyntReview>(5, JsonConvert.SerializeObject(movie));
                await foreach (var review in rewiews)
                {
                    Console.WriteLine(review.Text);
                }
                // Add movie
                var dbMovie = new Movie()
                {
                    Title = movie.Title,
                    Description = movie.Description
                };
                _context.Movies.Add(dbMovie);
                _context.SaveChanges();

                // Add ganres
                foreach (var ganre in movie.Ganres)
                {
                    var dbGanre = _context.Ganres.FirstOrDefault(x => x.Name.ToLower() == ganre.ToLower());
                    if (dbGanre == null)
                    {
                        dbGanre = new Ganre()
                        {
                            Name = ganre,
                        };
                        _context.Ganres.Add(dbGanre);
                    }

                    dbGanre.Movies.Add(dbMovie);
                    _context.SaveChanges();
                }
            }
        }
    }
}
