using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiChatApp.OpenAI;

public sealed class OpenAiClient
{
    private static readonly Uri ChatCompletionsEndpoint = new("https://api.openai.com/v1/chat/completions");

    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions serializerOptions;

    public OpenAiClient(string apiKey, HttpMessageHandler? handler = null)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key must be provided via OPENAI_API_KEY.");
        }

        httpClient = handler is null ? new HttpClient() : new HttpClient(handler);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<string> CreateChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken)
    {
        using var content = new StringContent(JsonSerializer.Serialize(request, serializerOptions), Encoding.UTF8, "application/json");
        using var response = await httpClient.PostAsync(ChatCompletionsEndpoint, content, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var chatResponse = await JsonSerializer.DeserializeAsync<ChatResponse>(stream, serializerOptions, cancellationToken).ConfigureAwait(false);
        if (chatResponse is null || chatResponse.Choices.Count == 0)
        {
            throw new InvalidOperationException("Empty response from OpenAI.");
        }

        return chatResponse.Choices[0].Message.Content;
    }
}