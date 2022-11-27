using System.Net;
using System.Net.Http.Json;

HttpClient httpClient = new HttpClient();

//await GetMethod();
//await GetByIdMethod();
//await AddMethod();
//await UpdateMethod();
await DeleteMethod();

async Task GetMethod()
{
    List<Person>? people = await httpClient.GetFromJsonAsync<List<Person>>("https://localhost:7269/api/users");
    if (people != null)
    {
        foreach (var person in people)
        {
            Console.WriteLine(person.Name);
        }
    }
}

async Task GetByIdMethod()
{
    // id первого объекта 
    int id = 1;
    using var response = await httpClient.GetAsync($"https://localhost:7269/api/users/{id}");
    // если объект на сервере найден, то есть статусный код равен 404
    if (response.StatusCode == HttpStatusCode.NotFound)
    {
        Error? error = await response.Content.ReadFromJsonAsync<Error>();
        Console.WriteLine(error?.Message);
    }
    else if (response.StatusCode == HttpStatusCode.OK)
    {
        // считываем ответ
        Person? person = await response.Content.ReadFromJsonAsync<Person>();
        Console.WriteLine($"{person?.Id} - {person?.Name}");
    }
}

async Task AddMethod()
{
    // отправляемый объект
    var mike = new Person { Name = "Mike", Age = 31 };
    using var response = await httpClient.PostAsJsonAsync("https://localhost:7269/api/users/", mike);
    // считываем ответ и десериализуем данные в объект Person
    Person? person = await response.Content.ReadFromJsonAsync<Person>();
    Console.WriteLine($"{person?.Id} - {person?.Name}");
}

async Task UpdateMethod()
{
    // id изменяемого объекта
    int id = 1;
    // отправляемый объект
    var tom = new Person { Id = id, Name = "Tomas", Age = 38 };
    using var response = await httpClient.PutAsJsonAsync("https://localhost:7269/api/users/", tom);
    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        // если возникла ошибка, считываем сообщение об ошибке
        Error? error = await response.Content.ReadFromJsonAsync<Error>();
        Console.WriteLine(error?.Message);
    }
    else if (response.StatusCode == System.Net.HttpStatusCode.OK)
    {
        // десериализуем ответ в объект Person
        Person? person = await response.Content.ReadFromJsonAsync<Person>();
        Console.WriteLine($"{person?.Id} - {person?.Name} ({person?.Age})");
    }
}

async Task DeleteMethod()
{
    // id удаляемого объекта
    int id = 1;

    using var response = await httpClient.DeleteAsync($"https://localhost:7269/api/users/{id}");
    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        // если возникла ошибка, считываем сообщение об ошибке
        Error? error = await response.Content.ReadFromJsonAsync<Error>();
        Console.WriteLine(error?.Message);
    }
    else if (response.StatusCode == System.Net.HttpStatusCode.OK)
    {
        // десериализуем ответ в объект Person
        Person? person = await response.Content.ReadFromJsonAsync<Person>();
        Console.WriteLine($"{person?.Id} - {person?.Name} ({person?.Age})");
    }
}

record Error(string Message);
class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Age { get; set; }
}