using GenAI;
using GenAI.Attributes;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;



AIGen.CacheModels = true;
AIGen.LoadModels();





var generator = AIGen.GetGenerator();
var cars = generator.Generate<Car>(3);
await foreach (var car in cars)
{
    Console.WriteLine("Model: " + car.Model);
    Console.WriteLine("Description: " + car.Description);
}









[Prompt("company")]
public class Company
{
    [Prompt("company official name")]
    public string Name { get; set; }

    [Prompt("address where company head office placed")]
    public string Address { get; set; }

    [Prompt("country where company head office placed")]
    public string Country { get; set; }

    [Random(1,10)]
    [Prompt("products names list that company developed")]
    public List<string> Products { get; set; } = new List<string>();
}

[Prompt("car")]
public class Car
{
    [Prompt("car brand name")]
    public string Brand { get; set; }

    [Prompt("car model from model line")]
    public string Model { get; set; }

    [Prompt("manufacturer company that develop car")]
    public Company Company { get; set; }

    [Prompt("car description")]
    public string Description { get; set; }
}


[Prompt("movie")]
public class Movie
{
    [Prompt("movie oficial title")]
    public string Title { get; set; }

    [Prompt("movie description")]
    public string Description { get; set; }

    [Prompt("recomended analogs for users who see this movie")]
    public Recomendations Recomendations { get; set; } = new Recomendations();
}

[Prompt("recomended analogs for users")]
public class Recomendations
{
    [Prompt("recomended book")]
    public string Books { get; set; }

    [Prompt("recomended movie")]
    public string Movies { get; set; }

    [Prompt("recomended game")]
    public string Games { get; set; }
}

[Prompt("review")]
public class Review
{
    [Prompt("review author name and surname")]
    public string Author { get; set; }

    [Prompt("review text")]
    public string Text { get; set; }

    [Prompt("review raiting")]
    public string Raiting { get; set; }
}
