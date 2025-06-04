using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

var sonarUrl = "https://sonar.example.com";
var token = "YOUR_TOKEN_HERE";

var projectKeys = new[] { "project_userservice", "project_orderservice", "project_productservice" };

var metrics = new[] { "coverage" };
var badgeBaseUrl = $"{sonarUrl}/api/project_badges/measure";

using var client = new HttpClient();

// Token jako Basic Auth
var authBytes = Encoding.ASCII.GetBytes($"{token}:");
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

Console.WriteLine("||Serwis||Coverage (%)||Badge||");

foreach (var projectKey in projectKeys)
{
    var metricKeys = string.Join(",", metrics);
    var url = $"{sonarUrl}/api/measures/component?component={projectKey}&metricKeys={metricKeys}";

    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"|{projectKey}|błąd|błąd|");
        continue;
    }

    var json = await response.Content.ReadAsStringAsync();
    using var doc = JsonDocument.Parse(json);
    var root = doc.RootElement;

    var measure = root
        .GetProperty("component")
        .GetProperty("measures")
        .EnumerateArray()
        .FirstOrDefault(m => m.GetProperty("metric").GetString() == "coverage");

    var value = measure.TryGetProperty("value", out var valElem) ? valElem.GetString() : "brak";

    var badgeUrl = $"{badgeBaseUrl}?project={projectKey}&metric=coverage&token={token}";

    Console.WriteLine($"|{projectKey}|{value}|!{badgeUrl}|height=20!|");
}