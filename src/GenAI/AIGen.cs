using GenAI.Attributes;
using GenAI.Inferences;
using Llama.Grammar.Helper;
using Llama.Grammar.Service;
using LLama.Common;
using LLama.Sampling;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GenAI
{
    public class AIGen
    {
        private void PropertiesToSchema(SchemaBuilder.PropertiesBuilder p, Type type)
        {
            foreach (var item in type.GetProperties())
            {
                var gen = item.GetCustomAttribute<PromptAttribute>();
                if (gen == null)
                {
                    continue;
                }

                // Check property type
                if (item.PropertyType.IsCollection())
                {
                    p.Add(item.Name, s => s.Type("array")
                           .Items(i => i.Type("string")));
                    continue;
                }

                if (item.PropertyType == typeof(string))
                {
                    p.Add(item.Name, s => s.Type("string"));
                    continue;
                }
            }
        }

        private Grammar CreateGrammar<T>() 
        {
            var type = typeof(T);
            var schemaBuilder = new SchemaBuilder().Type("object").Properties(p => PropertiesToSchema(p, type));   
            var json = schemaBuilder.ToJson();
            var grammar = new GbnfGrammar();
            var gbnf = grammar.ConvertJsonSchemaToGbnf(json);
            return new Grammar(gbnf, "root");
        } 

        private string CreatePrompt<T>()
        {
            var type = typeof(T);
            var objectInfo = type.GetCustomAttribute<PromptAttribute>();
            if (objectInfo == null)
            {
                throw new Exception("Class must be signed as GeneratedPromptAttribute");
            }

            var rules = new List<string>();
            foreach (var item in type.GetProperties())
            {
                var attribute = item.GetCustomAttribute<PromptAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                rules.Add($"- {item.Name}: {attribute.Description}");
            }

            if (!rules.Any())
            {
                throw new Exception("Class must contains at least 1 generated attribute");
            }

            return $"""
            Describe {objectInfo.Description} and paste into valid JSON

            Valid JSON rules:
            {string.Join("\n", rules)}
            """;
        }

        public async IAsyncEnumerable<T> Generate<T>(int count, string context = null) where T : new()
        {
            if (count <= 0)
            {
                yield break;
            }

            // Load Model
            var model = new TextModel();
            await model.LoadModel();

            // Add system prompt
            var prompt = CreatePrompt<T>();
            model.CreateSession(prompt);


            // Create generation rules
            var grammar = CreateGrammar<T>();
            var options = new InferenceParams()
            {
                MaxTokens = 1000,
                AntiPrompts = new List<string> { "User:" },
                SamplingPipeline = new DefaultSamplingPipeline
                {
                    Temperature = 0.6f,
                    Grammar = grammar
                }
            };

            // Generate objects
            int i = 0;
            while (i < count)
            {
                var text = await model.Generate($"Describe new one JSON");

                var json = Regex.Match(text, "\\{(.|\\s)*\\}");
                T? obj = default;

                // Parse json, if model generate uncorrect json recreate session
                try
                {
                    obj = JsonConvert.DeserializeObject<T>(json.Value);
                    if (obj == null)
                    {
                        throw new Exception();
                    }
                    i++;
                }
                catch (Exception)
                {                    
                    model.CreateSession(prompt);
                    continue;
                }

                yield return obj;
            }
        }
    }
}
