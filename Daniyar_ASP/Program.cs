using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    var path = context.Request.Path;
    var response = context.Response;

    if (path == "/postuser" && context.Request.Method == "POST")
    {
        var form = await context.Request.ReadFormAsync();
        string name = form["name"];
        string age = form["age"];
        await response.WriteAsync($"<div><p>Name: {name}</p>"
                                 + $"<p>Age: {age}</p></div>");
    }
    else if (path == "/image")
    {
        // Отправка изображения в ответ
        response.ContentType = "image/jpeg";
        await context.Response.SendFileAsync("image.jpg");
    }
    else if (path == "/download-image")
    {
        // Отправка изображения в ответ с заголовком Content-Disposition
        response.Headers["Content-Disposition"] = "attachment; filename=my_image.jpg";
        response.ContentType = "image/jpeg";
        await context.Response.SendFileAsync("image.jpg");
    }
    else if (File.Exists($"html/{path}"))
    {
        // Отправка запрошенной HTML-страницы
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync($"html/{path}");
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";

        // Обработка ошибки 404
        response.StatusCode = 404;
        await response.WriteAsync("<h2>Not Found</h2>");
    }
});

app.Run();
