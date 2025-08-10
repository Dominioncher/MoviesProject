using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.ModelsFilesLoading
{
    internal static class ModelsLoader
    {
        private static async Task DownloadGemma()
        {
            // Check model already download if use cache
            if (AIGen.CacheModels && File.Exists(Constants.TextModelPath))
            {
                return;
            }

            // Load model to TMP dir
            var modelUrl = "https://huggingface.co/google/gemma-3-4b-it-qat-q4_0-gguf/resolve/main/gemma-3-4b-it-q4_0.gguf?download=true";
            var localPath = Constants.TmpPath + "gemma-3-4b-it-q4_0.gguf";
            var loader = new FilesLoader();
            var progressBar = new ConsoleProgressBar(message: $"Download LLM model...");
            await loader.DownloadFileAsync(modelUrl, localPath, progressBar.GetProgress());

            // Move model to Models dir
            File.Move(localPath, Constants.TextModelPath, true);
        }

        public static void DownloadModels()
        {            
            Directory.CreateDirectory(Constants.TmpPath);
            Directory.CreateDirectory(Constants.ModelsPath);

            var gemma = DownloadGemma();
            Task.WaitAll(gemma);
        }
    }
}
