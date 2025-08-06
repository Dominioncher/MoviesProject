using LLama;
using LLama.Common;

namespace GenAI.Inferences
{
    public class TextModel
    {
        public LLamaWeights Model { get; private set; }

        private LLamaContext _context;

        private ChatSession _session;

        private ModelParams _params;

        public async Task LoadModel()
        {
            // Load model
            var modelPath = "Models/gemma-3-1b-it-UD-Q6_K_XL.gguf";
            _params = new ModelParams(modelPath)
            {
                GpuLayerCount = 4
            };
            Model = await LLamaWeights.LoadFromFileAsync(_params);
        }

        public void CreateSession(string prompt)
        {
            if (_context != null)
            {
                _context.Dispose();
            }

            _context = Model.CreateContext(_params);
            var executor = new InteractiveExecutor(_context);
            var history = new ChatHistory();
            history.AddMessage(AuthorRole.System, prompt);
            _session = new ChatSession(executor, history);
        }

        public async Task<string> Generate(string prompt, InferenceParams options = null)
        {                        
            // Create params
            var inferParams = options ?? new InferenceParams()
            {
                MaxTokens = 500,                
                AntiPrompts = new List<string> { "User:" }
            };

            var result = "";
            var chat = _session.ChatAsync(new ChatHistory.Message(AuthorRole.User, prompt), inferParams);
            await foreach (var text in chat)
            {
                result += text;
            }
            return result;
        }
    }
}
