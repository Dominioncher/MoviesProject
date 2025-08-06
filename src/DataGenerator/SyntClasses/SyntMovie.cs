using SixLabors.ImageSharp;
using GenAI.Attributes;

namespace DataGenerator.SyntClasses
{
    [Prompt("movie")]
    public class SyntMovie
    {
        [Prompt("movie title")]
        public string Title { get; set; }

        [Prompt("movie description")]
        public string Description { get; set; }

        [Prompt("movie ganres")]
        public List<string> Ganres { get; set; }

        [Random("01.01.2000", "01.01.2025")]
        public DateTime ReleaseDate { get; set; }

        [Random("00:30:00", "02:00:00")]
        public TimeSpan RunTime { get; set; }

        public Image Poster { get; set; }

        public List<Image> Screenshoots { get; set; }
    }
}
