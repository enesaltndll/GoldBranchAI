using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoldBranchAI.Services
{
    /// <summary>
    /// Gemini API'den dönen her bir alt görev
    /// </summary>
    public class AiSubTask
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("estimatedHours")]
        public int EstimatedHours { get; set; } = 2;

        [JsonPropertyName("priority")]
        public string Priority { get; set; } = "medium";

        [JsonPropertyName("deadlineDays")]
        public int DeadlineDays { get; set; } = 7;
    }

    public class AiBreakdownResult
    {
        [JsonPropertyName("tasks")]
        public List<AiSubTask> Tasks { get; set; } = new();
    }

    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiService> _logger;

        public GeminiService(HttpClient httpClient, IConfiguration configuration, ILogger<GeminiService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Büyük bir proje açıklamasını Gemini AI ile alt görevlere böler ve zamanlandırır.
        /// </summary>
        public async Task<(List<AiSubTask> Tasks, string RawJson)> BreakdownProjectAsync(string projectDescription)
        {
            var apiKey = _configuration["GeminiApi:ApiKey"];
            var model = _configuration["GeminiApi:Model"] ?? "gemini-2.5-flash";

            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("Gemini API anahtarı bulunamadı. appsettings.json dosyasını kontrol edin.");

            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var systemPrompt = @"Sen üst düzey bir teknik proje yöneticisisin. Sana verilen proje tanımını analiz et ve onu yazılım geliştirme alt görevlerine böl. 

ÖNEMLİ KURALLAR:
1. Projenin genel büyüklüğünü ve zorluğunu hayal et. Gerekirse haftalara/aylara yayılacak şekilde mantıksal sprintler oluştur. Bütün projeyi 1 haftaya sığdırmak zorunda değilsin.
2. Görevler mantıksal sıraya göre dizilmeli (Önce altyapı/DB, sonra özellikler, UI, en son test/yayına alma).
3. 'estimatedHours': Bir görev için harcanacak aktif çalışma saatidir (1-16 arası).
4. 'deadlineDays': Bu görevin bugünden itibaren KAÇ GÜN SONRA bitirilmiş olması gerektiğini belirler (Örn: Altyapı için 3, Core özellikler için 14, Test için 30 gibi). Bu değeri projenin uzunluğuna göre DAĞIT, hepsini aynı güne yığma!
5. Öncelik: ""high"" (kritik), ""medium"" (ana özellikler), ""low"" (eklemeler).
6. En az 3, en fazla 15 görev üret.

SADECE aşağıdaki JSON formatında yanıt ver, başka hiçbir metin (markdown vb) ekleme:
{
  ""tasks"": [
    {
      ""title"": ""Müşteri Veritabanı Altyapısı"",
      ""description"": ""Detaylı teknik açıklama"",
      ""estimatedHours"": 6,
      ""priority"": ""high"",
      ""deadlineDays"": 3
    }
  ]
}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = $"{systemPrompt}\n\nPROJE TANIMI:\n{projectDescription}" }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 4096,
                    responseMimeType = "application/json"
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Gemini API hatası: {StatusCode} - {Body}", response.StatusCode, responseBody);
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    throw new HttpRequestException("API Kotası Doldu veya Bölge Kısıtlaması (Limit 0). Google Cloud Console üzerinden projenize 'Billing' (Fatura) hesabı eklemeniz gerekebilir.");
                }
                throw new HttpRequestException($"Gemini API hatası ({response.StatusCode})");
            }

            var geminiResponse = JsonDocument.Parse(responseBody);
            var textContent = geminiResponse.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            if (string.IsNullOrEmpty(textContent))
                throw new InvalidOperationException("Gemini API boş yanıt döndü.");

            var result = JsonSerializer.Deserialize<AiBreakdownResult>(textContent);

            if (result?.Tasks == null || result.Tasks.Count == 0)
                throw new InvalidOperationException("Gemini API geçerli görev listesi üretemedi.");

            return (result.Tasks, textContent);
        }

        /// <summary>
        /// Geliştiriciler için AI kod ve araştırma asistanı.
        /// </summary>
        public async Task<string> AskDeveloperQuestionAsync(string question)
        {
            var apiKey = _configuration["GeminiApi:ApiKey"];
            var model = _configuration["GeminiApi:Model"] ?? "gemini-2.5-flash";

            if (string.IsNullOrEmpty(apiKey)) return "HATA: API Anahtarı bulunamadı.";

            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var systemPrompt = @"Sen deneyimli, çözüm odaklı, usta bir yazılım geliştiricisisin.
Sana yazılım, hata ayıklama (debugging), algoritma mantığı veya teknoloji ile ilgili sorular sorulacak.
Açıklamalarını gereksiz uzatma, doğrudan sorunun veya konunun kalbine in.
Gerekiyorsa Markdown formatında bolca kod örneği ver. Türkçeyi akıcı ve modern bir teknoloji diliyle kullan.";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = $"{systemPrompt}\n\nGELİŞTİRİCİ SORUSU:\n{question}" }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.5,
                    maxOutputTokens = 8192
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"HATA: Gemini API Bağlantı Hatası ({response.StatusCode})";
            }

            try
            {
                var geminiResponse = JsonDocument.Parse(responseBody);
                var textContent = geminiResponse.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return textContent ?? string.Empty;
            }
            catch
            {
                return "API'den okunamayan bir format döndü.";
            }
        }
    }
}
