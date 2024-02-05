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

                string message = "������������ ������";

                try
                {
                    var formData = await request.ReadFormAsync(); // ������ ������ �����
                    var firstName = formData["firstName"];
                    var lastName = formData["lastName"];
                    var age = formData["age"];

                    message = $"���: {firstName}, �������: {lastName}, �������: {age}";
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
                var htmlFilePath = "html/showimage.html"; // ���� � ������ ����� HTML

                // ������ ����������� HTML-�����
                var htmlContent = await File.ReadAllTextAsync(htmlFilePath);

                // ��������� MIME-���� ��� HTML-��������
                response.Headers["Content-Type"] = "text/html; charset=utf-8";

                // �������� HTML-�������� �������
                await response.WriteAsync(htmlContent);
            });


            // ��������� ��������� ���������� ��� ����������� ����������� ��� HTML-����
            app.MapGet("/showimagefile", async context =>
            {
                var response = context.Response;
                var imagePath = "html/�.png"; // ���� � ������ �����������

                // ��������� MIME-���� ��� �����������
                response.Headers["Content-Type"] = "image/png";

                // �������� �����������
                await response.SendFileAsync(imagePath);
            });


            app.MapGet("/downloadimage", async context =>
            {
                var response = context.Response;
                var imagePath = "html/�.png"; // ���� � ������ �����������

                // ��������� ����� ����� �� ASCII-�����������
                var asciiCompatibleFilename = "image.png";

                // ��������� ��������� ��� ���������� �����
                response.Headers["Content-Disposition"] = $"attachment; filename=\"{asciiCompatibleFilename}\"";

                // �������� ����� ��� ����������
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
                var htmlFilePath = "html/pagenotfound.html"; // ���� � ������ ����� HTML

                // ������ ����������� HTML-�����
                var htmlContent = await File.ReadAllTextAsync(htmlFilePath);

                // ��������� MIME-���� ��� HTML-��������
                response.Headers["Content-Type"] = "text/html; charset=utf-8";

                // ��������� ���������� ���� 404
                response.StatusCode = 404;

                // �������� HTML-�������� �������
                await response.WriteAsync(htmlContent);
            });


            app.MapGet("/", async context =>
            {
                var response = context.Response;
                response.ContentType = "text/html; charset=utf-8";
                await response.WriteAsync("����� ���������� �� ������� ��������!<br>");
                await response.WriteAsync("<a href=\"/showform\">������� � �������� �����</a>!<br>");
                await response.WriteAsync("<a href=\"/showimage\">������� � �������� ��������</a>!");

            });

            app.Run();
        }
    }
}
