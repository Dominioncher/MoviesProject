using SixLabors.ImageSharp;
using AIDataGen.Core.Attributes;

namespace DataGenerator.SyntClasses
{
    [Prompt("Movies")]
    public class SyntMovie
    {
        [Prompt("Movie title")]
        public string Title { get; set; }

        [Prompt("Movie description")]
        public string Description { get; set; }

        [Random(1, 4)]
        [Prompt("Movie ganres")]
        public List<string> Ganres { get; set; }

        [Prompt("Movie poster")]
        public Image Poster { get; set; }

        [Random(1,5)]
        [Prompt("Movie screenshoots")]
        public List<Image> Screenshoots { get; set; }
    }
}
