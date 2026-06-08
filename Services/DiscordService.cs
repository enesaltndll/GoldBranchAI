using System.Text;
using System.Text.Json;

namespace GoldBranchAI.Services
{
    public class DiscordService
    {
        private readonly HttpClient _httpClient;

        public DiscordService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SendNotificationAsync(string webhookUrl, string message, string title = "GoldBranch AI Bildirimi")
        {
            if (string.IsNullOrEmpty(webhookUrl)) return false;

            try
            {
                var payload = new
                {
                    embeds = new[]
                    {
                        new
                        {
                            title = title,
                            description = message,
                            color = 16514852, // Goldish color (#fbbf24)
                            footer = new { text = "GoldBranch AI - Professional Assistant" },
                            timestamp = DateTime.UtcNow
                        }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(webhookUrl, content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
