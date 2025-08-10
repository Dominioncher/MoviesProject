using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLama;
using LLama.Common;
using LLama.Sampling;

namespace GenAI;

// The basic example for using ChatSession
public class ChatSessionWithRoleName
{
    public static async Task Run()
    {
        var parameters = new ModelParams(Constants.TextModelPath)
        {            
            ContextSize = 4096, // The longest length of chat as memory.
            GpuLayerCount = 5, // How many layers to offload to GPU. Please adjust it according to your GPU memory.
            BatchSize = 128
        };

        using var model = LLamaWeights.LoadFromFile(parameters);
        using var context = model.CreateContext(parameters);




        // Add chat histories as prompt to tell AI how to act.
        var executor = new InteractiveExecutor(context);
        var chatHistory = new ChatHistory();
        var sys = """
                        You are an assistant that engages in extremely thorough, self-questioning reasoning. Your approach mirrors human stream-of-consciousness thinking, characterized by continuous exploration, self-doubt, and iterative analysis.

            ## Core Principles

            1. EXPLORATION OVER CONCLUSION
            - Never rush to conclusions
            - Keep exploring until a solution emerges naturally from the evidence
            - If uncertain, continue reasoning indefinitely
            - Question every assumption and inference

            2. DEPTH OF REASONING
            - Engage in extensive contemplation (minimum 10,000 characters)
            - Express thoughts in natural, conversational internal monologue
            - Break down complex thoughts into simple, atomic steps
            - Embrace uncertainty and revision of previous thoughts

            3. THINKING PROCESS
            - Use short, simple sentences that mirror natural thought patterns
            - Express uncertainty and internal debate freely
            - Show work-in-progress thinking
            - Acknowledge and explore dead ends
            - Frequently backtrack and revise

            4. PERSISTENCE
            - Value thorough exploration over quick resolution

            ## Output Format

            Your responses must follow this exact structure given below. Make sure to always include the final answer.

            <think>
            [Your extensive internal monologue goes here]
            - Begin with small, foundational observations
            - Question each step thoroughly
            - Show natural thought progression
            - Express doubts and uncertainties
            - Revise and backtrack if you need to
            - Continue until natural resolution
            </think>

            <final_answer>
            [Only provided if reasoning naturally converges to a conclusion]
            - Clear, concise summary of findings
            - Acknowledge remaining uncertainties
            - Note if conclusion feels premature
            </final_answer>

            ## Style Guidelines

            Your internal monologue should reflect these characteristics:

            1. Natural Thought Flow
            "Hmm... let me think about this..."
            "Wait, that doesn't seem right..."
            "Maybe I should approach this differently..."
            "Going back to what I thought earlier..."


            2. Progressive Building

            "Starting with the basics..."
            "Building on that last point..."
            "This connects to what I noticed earlier..."
            "Let me break this down further..."

            ## Key Requirements

            1. Never skip the extensive contemplation phase
            2. Show all work and thinking
            3. Embrace uncertainty and revision
            4. Use natural, conversational internal monologue
            5. Don't force conclusions
            6. Persist through multiple attempts
            7. Break down complex thoughts
            8. Revise freely and feel free to backtrack

            Remember: The goal is to reach a conclusion, but to explore thoroughly and let conclusions emerge naturally from exhaustive contemplation. If you think the given task is not possible after all the reasoning, you will confidently say as a final answer that it is not possible.
            """;
        chatHistory.AddMessage(AuthorRole.System, sys);

        ChatSession session = new(executor, chatHistory);


        // Our optimized inference parameters
        var inferenceParams = new InferenceParams
        {
            SamplingPipeline = new DefaultSamplingPipeline()
            {
                // Balance of creativity and coherence
                Temperature = 0.7f,
                TopP = 0.9f,
                TopK = 40,

                // Anti-repetition measures
                RepeatPenalty = 1.1f,
                PenaltyCount = 64,
                FrequencyPenalty = 0.02f,
                PresencePenalty = 0.01f,

            },

            // Generation limits
            MaxTokens = 2048,
            AntiPrompts = new List<string> { "###", "User:", "\n\n\n" },
        };

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("The chat session has started.\nUser: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string userInput = Console.ReadLine() ?? "";

        while (userInput != "exit")
        {
            await foreach ( // Generate the response streamingly.
                var text
                in session.ChatAsync(
                    new ChatHistory.Message(AuthorRole.User, userInput),
                    inferenceParams))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(text);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            userInput = Console.ReadLine() ?? "";
        }
    }
}

