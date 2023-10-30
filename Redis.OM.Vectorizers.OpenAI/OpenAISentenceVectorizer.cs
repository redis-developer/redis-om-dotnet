using System.Net.Http.Json;
using System.Text.Json;
using Redis.OM.Contracts;
using Redis.OM.Modeling;

namespace Redis.OM.Vectorizers.OpenAI;

public class OpenAISentenceVectorizer : IVectorizer<string>
{
    private readonly string _openAIAuthToken;
    private readonly string _model;

    public OpenAISentenceVectorizer(string openAIAuthToken, string model = "text-embedding-ada-002", int dim = 1536)
    {
        _openAIAuthToken = openAIAuthToken;
        _model = model;
        Dim = dim;
    }

    public VectorType VectorType => VectorType.FLOAT32;
    public int Dim { get; }
    public byte[] Vectorize(string str)
    {
        var floats = GetFloats(str, _model, _openAIAuthToken);
        return floats.SelectMany(BitConverter.GetBytes).ToArray();
    }

    internal static float[] GetFloats(string s, string model, string openAIAuthToken)
    {
        var client = Configuration.Instance.Client;
        var requestContent = JsonContent.Create(new { input = s, model = model });

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{Configuration.Instance.OpenAiApiUrl}/v1/embeddings"),
            Content = requestContent,
            Headers = { { "Authorization", $"Bearer {openAIAuthToken}" } }
        };

        var res = client.Send(request);
        if (!res.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Open AI did not respond with a positive error code: {res.StatusCode}, {res.ReasonPhrase}");
        }

        var jsonObj = JsonSerializer.Deserialize<JsonElement>(RedisOMHttpUtil.ReadJsonSync(res));

        
        if (!jsonObj.TryGetProperty("data", out var data))
        {
            throw new Exception("Malformed Response");
        }

        if (data.GetArrayLength() < 1 ||  !data[0].TryGetProperty("embedding", out var embedding))
        {
            throw new Exception("Malformed Response");
        }

        return embedding.Deserialize<float[]>()!;
    }
}