using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task2.DataBase;
using Task2.Excel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDBContext>();
builder.Services.AddScoped<ExcelService>();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/excel/files/upload", async ([FromServices] ExcelService service, IFormFile file) =>
{
    var isLoaded = await service.ImportIntoDB(file); 
    return isLoaded ? Results.Ok("File was loaded") : Results.Problem("File wasn't loaded");
}).DisableAntiforgery();

app.MapGet("/excel/files", async ([FromServices] ExcelService service) =>
{
    var files = await service.GetFiles();
    return files is not null ? Results.Ok(files) : Results.Problem("Files weren't found");
});

app.MapGet("/excel/files/{fileId:int}", async ([FromServices] ExcelService service, int fileId) =>
{
    var file = await service.GetFile(fileId);
    return file is not null ? Results.Ok(file) : Results.Problem("File wasn't found");

});
app.Run();
