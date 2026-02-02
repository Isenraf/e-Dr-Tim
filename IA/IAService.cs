using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Markdig;

namespace BANA.IA
{
    public interface IAIService
    {
        Task<string> AskAsync(string prompt);
    }

    public class GeminiService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly string _baseUrl = "https://generativelanguage.googleapis.com/v1beta/models/";

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["AI:Gemini:ApiKey"];
            _model = configuration["AI:Gemini:Model"] ?? "gemini-pro";

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("Gemini API Key is not configured.");
            }
        }

        public async Task<string> AskAsync(string prompt)
        {
            var endpoint = $"{_baseUrl}{_model}:generateContent?key={_apiKey}";

            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Log error or handle it gracefully
                throw new Exception($"Gemini API Error {response.StatusCode}: {responseText}");
            }

            try 
            {
                var result = JObject.Parse(responseText);
                string answer = result["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();
                
                if (string.IsNullOrEmpty(answer))
                {
                    return "No response from AI.";
                }

                // Convert Markdown to HTML as done in original code
                string html = Markdown.ToHtml(answer);
                return html;
            }
            catch (Exception ex)
            {
                 throw new Exception($"Error parsing Gemini response: {ex.Message}");
            }
        }
    }
}
