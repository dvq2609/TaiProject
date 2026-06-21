using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Text.Json;

namespace BE.Controllers
{
    [Route("api/aichat")]
    [ApiController]
    public class AiChatController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static readonly HttpClient _httpClient = new HttpClient();

        public AiChatController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("ask")]
        [Authorize] // Chỉ user đã đăng nhập mới được dùng AI chat
        public async Task<IActionResult> Ask([FromBody] AiChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserMsg))
            {
                return BadRequest(new { Message = "Yêu cầu không hợp lệ hoặc tin nhắn trống." });
            }

            var apiKey = _configuration["AiChat:ApiKey"];
            var baseUrl = _configuration["AiChat:BaseUrl"];
            var configuredModel = _configuration["AiChat:Model"];

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(baseUrl))
            {
                return StatusCode(500, new { Message = "Chưa cấu hình API Key hoặc Base URL cho chatbot AI trong hệ thống." });
            }

            try
            {
                bool isGemini = baseUrl.Contains("generativelanguage", StringComparison.OrdinalIgnoreCase) || 
                                baseUrl.Contains("gemini", StringComparison.OrdinalIgnoreCase);

                string responseContent;

                if (isGemini)
                {
                    // Gemini Endpoint Format
                    string url;
                    if (baseUrl.Contains("generateContent"))
                    {
                        url = $"{baseUrl}?key={apiKey}";
                    }
                    else
                    {
                        var cleanBase = baseUrl.TrimEnd('/');
                        var model = string.IsNullOrEmpty(configuredModel) ? "gemini-1.5-flash-latest" : configuredModel;
                        url = $"{cleanBase}/v1beta/models/{model}:generateContent?key={apiKey}";
                    }

                    // Format prompt
                    var historyContext = string.Join("\n", request.History.TakeLast(6).Select(m => $"{(m.IsBot ? "AI" : "User")}: {m.Text}"));
                    var prompt = $"Bạn là AI tư vấn trồng cây của VƯƠN. Trả lời ngắn gọn, phù hợp khí hậu nhiệt đới Việt Nam.\n\n{historyContext}\n\nUser: {request.UserMsg}\n\nAI:";

                    var payload = new
                    {
                        contents = new[]
                        {
                            new { parts = new[] { new { text = prompt } } }
                        },
                        generationConfig = new
                        {
                            temperature = 0.7,
                            maxOutputTokens = 1000
                        }
                    };

                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
                    };

                    var response = await _httpClient.SendAsync(requestMessage);
                    responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        return StatusCode((int)response.StatusCode, new { Message = $"Lỗi từ AI Provider (Gemini): {responseContent}" });
                    }

                    using var doc = JsonDocument.Parse(responseContent);
                    var reply = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    return Ok(new { Reply = reply });
                }
                else
                {
                    // OpenAI-Compatible Format
                    string url;
                    if (baseUrl.Contains("chat/completions"))
                    {
                        url = baseUrl;
                    }
                    else
                    {
                        var cleanBase = baseUrl.TrimEnd('/');
                        url = $"{cleanBase}/v1/chat/completions";
                    }

                    var model = string.IsNullOrEmpty(configuredModel) ? "gpt-4o-mini" : configuredModel;

                    var messages = new List<object>
                    {
                        new { role = "system", content = "Bạn là AI tư vấn trồng cây của VƯƠN. Trả lời ngắn gọn, phù hợp khí hậu nhiệt đới Việt Nam." }
                    };

                    foreach (var h in request.History.TakeLast(6))
                    {
                        messages.Add(new { role = h.IsBot ? "assistant" : "user", content = h.Text });
                    }
                    messages.Add(new { role = "user", content = request.UserMsg });

                    var payload = new
                    {
                        model = model,
                        messages = messages,
                        temperature = 0.7,
                        max_tokens = 1000
                    };

                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
                    };
                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                    var response = await _httpClient.SendAsync(requestMessage);
                    responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        return StatusCode((int)response.StatusCode, new { Message = $"Lỗi từ AI Provider (OpenAI-compatible): {responseContent}" });
                    }

                    using var doc = JsonDocument.Parse(responseContent);
                    var reply = doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    return Ok(new { Reply = reply });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Lỗi hệ thống khi kết nối Chatbot: {ex.Message}" });
            }
        }
    }

    public class AiChatRequest
    {
        public string UserMsg { get; set; } = string.Empty;
        public List<ChatHistoryItem> History { get; set; } = new List<ChatHistoryItem>();
    }

    public class ChatHistoryItem
    {
        public string Text { get; set; } = string.Empty;
        public bool IsBot { get; set; }
    }
}
