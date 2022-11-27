using System.Net.Http.Headers;
using Newtonsoft.Json;

using var client = new HttpClient();

client.BaseAddress = new Uri("https://api.github.com");
client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

var url = "repos/symfony/symfony/contributors";
HttpResponseMessage response = await client.GetAsync(url);
response.EnsureSuccessStatusCode();
var resp = await response.Content.ReadAsStringAsync();

List<Contributor> contributors = JsonConvert.DeserializeObject<List<Contributor>>(resp);
contributors.ForEach(Console.WriteLine);

record Contributor(string Login, short Contributions);