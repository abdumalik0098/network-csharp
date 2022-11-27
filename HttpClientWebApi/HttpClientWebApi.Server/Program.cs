int id = 1; // дл€ генерации id объектов
// начальные данные
List<Person> users = new List<Person>
{
    new() { Id = id++, Name = "Tom", Age = 37 },
    new() { Id = id++, Name = "Bob", Age = 41 },
    new() { Id = id++, Name = "Sam", Age = 24 }
};

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.MapGet("/api/users", () => users);

app.MapGet("/api/users/{id}", (int id) =>
{
    // получаем пользовател€ по id
    Person? user = users.FirstOrDefault(u => u.Id == id);
    // если не найден, отправл€ем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "ѕользователь не найден" });

    // если пользователь найден, отправл€ем его
    return Results.Json(user);
});

app.MapDelete("/api/users/{id}", (int id) =>
{
    // получаем пользовател€ по id
    Person? user = users.FirstOrDefault(u => u.Id == id);

    // если не найден, отправл€ем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "ѕользователь не найден" });

    // если пользователь найден, удал€ем его
    users.Remove(user);
    return Results.Json(user);
});

app.MapPost("/api/users", (Person user) =>
{

    // устанавливаем id дл€ нового пользовател€
    user.Id = id++;
    // добавл€ем пользовател€ в список
    users.Add(user);
    return user;
});

app.MapPut("/api/users", (Person userData) =>
{

    // получаем пользовател€ по id
    var user = users.FirstOrDefault(u => u.Id == userData.Id);
    // если не найден, отправл€ем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "ѕользователь не найден" });
    // если пользователь найден, измен€ем его данные и отправл€ем обратно клиенту

    user.Age = userData.Age;
    user.Name = userData.Name;
    return Results.Json(user);
});

app.Run();

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Age { get; set; }
}