using GenAI;
using GenAI.Attributes;

namespace GenAITests
{
    [TestClass]
    public sealed class Test1
    {
        [Prompt("car")]
        public class Car
        {
            [Prompt("car brand name")]
            public string Brand { get; set; }

            [Prompt("car model from model line")]
            public string Model { get; set; }

            [Prompt("manufacturer company")]
            public string Company { get; set; }

            [Prompt("description about model")]
            public string Description { get; set; }

            [Prompt("list of owners reviews about car")]
            public List<string> Reviews {get;set;}
        }

        public class Review
        {
            [Prompt("car model from model line")]
            public string Text { get; set; }
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var generator = new AIGen();

            var carsList = new List<Car>();
            var cars = generator.Generate<Car>(2);
            await foreach (var car in cars)
            {
                carsList.Add(car);
            }



        }
    }
}
