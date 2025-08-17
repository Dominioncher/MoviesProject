using AIDataGen.Core.Attributes;

namespace DataGenerator.SyntClasses
{
    [Prompt("Rewiews")]
    public class SyntReview
    {
        [Prompt("Author nickname")]
        public string Author { get; set; }

        [Prompt("Rewiew text", "Not less 100 words")]
        public string Text { get; set; }
    }
}
