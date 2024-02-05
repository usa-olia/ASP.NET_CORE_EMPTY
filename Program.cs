using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASP.NET_CORE_EMPTY
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapPost("/api/user", async context =>
            {
                var response = context.Response;
                var request = context.Request;

                string message = "Некорректный запрос";

                try
                {
                    var formData = await request.ReadFormAsync(); // Чтение данных формы
                    var firstName = formData["firstName"];
                    var lastName = formData["lastName"];
                    var age = formData["age"];

                    message = $"Имя: {firstName}, Фамилия: {lastName}, Возраст: {age}";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                response.ContentType = "application/json";
                await response.WriteAsJsonAsync(new { text = message });
            });

            app.MapGet("/showimage", async context =>
            {
                var response = context.Response;
                var htmlFilePath = "html/showimage.html"; // Путь к вашему файлу HTML

                // Чтение содержимого HTML-файла
                var htmlContent = await File.ReadAllTextAsync(htmlFilePath);

                // Установка MIME-типа для HTML-страницы
                response.Headers["Content-Type"] = "text/html; charset=utf-8";

                // Отправка HTML-страницы клиенту
                await response.WriteAsync(htmlContent);
            });


            // Добавляем отдельный обработчик для отображения изображения без HTML-кода
            app.MapGet("/showimagefile", async context =>
            {
                var response = context.Response;
                var imagePath = "html/С.png"; // Путь к вашему изображению

                // Установка MIME-типа для изображения
                response.Headers["Content-Type"] = "image/png";

                // Отправка изображения
                await response.SendFileAsync(imagePath);
            });


            app.MapGet("/downloadimage", async context =>
            {
                var response = context.Response;
                var imagePath = "html/С.png"; // Путь к вашему изображению

                // Изменение имени файла на ASCII-совместимое
                var asciiCompatibleFilename = "image.png";

                // Установка заголовка для скачивания файла
                response.Headers["Content-Disposition"] = $"attachment; filename=\"{asciiCompatibleFilename}\"";

                // Отправка файла для скачивания
                await response.SendFileAsync(imagePath);
            });


            app.MapGet("/showform", async context =>
            {
                var response = context.Response;
                response.ContentType = "text/html; charset=utf-8";
                await response.SendFileAsync("html/index.html");
            });

            app.MapGet("/{*url}", async context =>
            {
                var response = context.Response;
                var htmlFilePath = "html/pagenotfound.html"; // Путь к вашему файлу HTML

                // Чтение содержимого HTML-файла
                var htmlContent = await File.ReadAllTextAsync(htmlFilePath);

                // Установка MIME-типа для HTML-страницы
                response.Headers["Content-Type"] = "text/html; charset=utf-8";

                // Установка статусного кода 404
                response.StatusCode = 404;

                // Отправка HTML-страницы клиенту
                await response.WriteAsync(htmlContent);
            });


            app.MapGet("/", async context =>
            {
                var response = context.Response;
                response.ContentType = "text/html; charset=utf-8";
                await response.WriteAsync("Добро пожаловать на главную страницу!<br>");
                await response.WriteAsync("<a href=\"/showform\">Перейти к проверки формы</a>!<br>");
                await response.WriteAsync("<a href=\"/showimage\">Перейти к проверки картинки</a>!");

            });

            app.Run();
        }
    }
}
