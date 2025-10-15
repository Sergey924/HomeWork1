using System.Text;
using AiChatApp.OpenAI;

Console.OutputEncoding = Encoding.UTF8;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine("OPENAI_API_KEY не задан. Установите переменную окружения и повторите.");
    Environment.Exit(1);
}

var model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

var client = new OpenAiClient(apiKey!);
var messages = new List<ChatMessage>
{
    new ChatMessage { Role = "system", Content = "You are a helpful assistant. Answer concisely." }
};

Console.WriteLine($"Модель: {model}. Введите сообщение (exit/quit/q для выхода).");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if (input is null) break;
    input = input.Trim();
    if (input.Length == 0) continue;
    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
        input.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
        input.Equals("q", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    messages.Add(new ChatMessage { Role = "user", Content = input });

    try
    {
        var request = new ChatRequest
        {
            Model = model,
            Messages = messages,
            Temperature = 0.7
        };

        var reply = await client.CreateChatCompletionAsync(request, CancellationToken.None);
        Console.WriteLine(reply);

        messages.Add(new ChatMessage { Role = "assistant", Content = reply });
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Ошибка запроса: {ex.Message}");
    }
}
