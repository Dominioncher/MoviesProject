using GenAI.Attributes;
using GenAI.Inferences;
using GenAI.ModelsFilesLoading;
using Llama.Grammar.Helper;
using Llama.Grammar.Service;
using LLama.Common;
using LLama.Native;
using LLama.Sampling;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GenAI
{
    public class AIGen
    {
        public static bool CacheModels { get; set; } = false;

        private static AIGen _instance;

        static AIGen() 
        {
            NativeLogConfig.llama_log_set((level, message) =>
            {
            });
        }

        public static void LoadModels()
        {
            ModelsLoader.DownloadModels();
        }

        public static AIGen GetGenerator()
        {
            if (_instance != null)
            {
                return _instance;
            }

            var instanse = new AIGen();
            instanse.LoadModel().Wait();
            return instanse;
        }


        private TextModel _model;

        private async Task LoadModel()
        {
            _model = new TextModel();
            await _model.LoadModel();
        }


        public async IAsyncEnumerable<T> Generate<T>(int count, string context = null) where T : new()
        {
            if (count <= 0)
            {
                throw new Exception("Count must be > 0");
            }

            var type = typeof(T);
            var attribute = type.GetCustomAttribute<PromptAttribute>();
            var prompt = Prompts.GetDataGenerationPrompt(attribute.Description, context);//, modifiers: new string[] { "Get detailed description" });
            using var session = _model.CreateSession();

            for (int i = 1; i < count; i++)
            {
                var text = await session.Generate(prompt);
                var instance = await TextToObject(type, text, attribute.Description, session);
                yield return (T)instance;
            }
        }

        private async ValueTask<object> GenerateString(PropertyInfo info, string parentAttribute, string parentDescription, TextModelSession session)
        {
            var attribute = info.GetCustomAttribute<PromptAttribute>();
            var prompt = Prompts.GetDataGenerationPrompt(attribute.Description, parentAttribute, parentDescription);
            var text = await session.Generate(prompt);
            return await TextToObject(info.PropertyType, text, attribute.Description, session);
        }

        private async ValueTask<object> GenerateCollection(PropertyInfo info, string parentAttribute, string parentDescription, TextModelSession session)
        {
            var attribute = info.GetCustomAttribute<PromptAttribute>();
            var randomAttribute = info.GetCustomAttribute<RandomAttribute>();
            var rnd = new Random();
            var count = rnd.Next((int)randomAttribute.Min, (int)randomAttribute.Max);
            var instance = Activator.CreateInstance(info.PropertyType);
            var elementType = info.PropertyType.GetGenericArguments()[0];

            if (count <= 0)
            {
                return instance;
            }

            var prompt = Prompts.GetListGenerationPrompt(attribute.Description, parentAttribute, parentDescription);
            
            for (int i = 0; i < count; i++)
            {
                var text = await session.Generate(prompt);
                var element = await TextToObject(elementType, text, attribute.Description, session);
                info.PropertyType.GetMethod("Add").Invoke(instance, [element]);
            }

            return instance;
        }

        private async ValueTask<object> GenerateClass(PropertyInfo info, string parentAttribute, string parentDescription, TextModelSession session)
        {
            var attribute = info.GetCustomAttribute<PromptAttribute>();
            var prompt = Prompts.GetDataGenerationPrompt(attribute.Description, parentAttribute, parentDescription);//, modifiers: new string[] { "Get detailed description" });
            var text = await session.Generate(prompt);
            return await TextToObject(info.PropertyType, text, attribute.Description, session);
        }

        private async ValueTask<object> TextToObject(Type type, string text, string description, TextModelSession session)
        {
            if (type == typeof(string))
            {
                return text;
            }

            if (type.IsClass)
            {
                var instance = Activator.CreateInstance(type);
                foreach (var property in type.GetProperties())
                {
                    var propertyInstance = await GenerateProperty(property, description, text, session);
                    property.SetValue(instance, propertyInstance);
                }
                return instance;
            }

            return null;
        }

        private async ValueTask<object> GenerateProperty(PropertyInfo info, string parentAttribute, string parentDescription, TextModelSession session)
        {
            if (info.PropertyType.IsCollection())
            {
                return await GenerateCollection(info, parentAttribute, parentDescription, session);
            }

            if (info.PropertyType == typeof(string))
            {
                return await GenerateString(info, parentAttribute, parentDescription, session);
            }

            if (info.PropertyType.IsClass)
            {
                return await GenerateClass(info, parentAttribute, parentDescription, session);
            }

            return null;
        }
    }
}
