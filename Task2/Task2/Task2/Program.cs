using Microsoft.AspNetCore.Mvc;
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

app.MapGet("/excel/files/{fileId:int}", async (int fileId) =>
{
    return Results.Problem("Not implemented");
});
app.Run();
