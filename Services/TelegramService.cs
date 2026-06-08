using System.Text;
using System.Text.Json;

namespace GoldBranchAI.Services
{
    public class TelegramService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public TelegramService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        private string BotToken => _config["Telegram:BotToken"] ?? "8952079382:AAGyoifTYN0hCmATdxOWEcFvStZhT0iRw1c";

        public async Task<bool> SendMessageAsync(string chatId, string message)
        {
            if (string.IsNullOrEmpty(chatId) || BotToken.Contains("YOUR_BOT_TOKEN")) return false;

            try
            {
                var url = $"https://api.telegram.org/bot{BotToken}/sendMessage";
                var payload = new
                {
                    chat_id = chatId,
                    text = message,
                    parse_mode = "HTML"
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"TELEGRAM ERROR: {errorJson}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TELEGRAM EXCEPTION: {ex.Message}");
                return false;
            }
        }
    }
}
