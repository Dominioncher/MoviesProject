using GenAI.Attributes;

namespace DataGenerator.SyntClasses
{
    [Prompt("rewiew")]
    public class SyntReview
    {
        [Prompt("author nickname")]
        public string Author { get; set; }

        [Prompt("rewiew in 100 words")]
        public string Text { get; set; }

        [Random(1,5)]
        public int Raiting { get; set; }
    }
}
