using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Task2.DataBase;
using Task2.Excel;

var builder = WebApplication.CreateBuilder(args);

// Add the application's database context to the service collection
builder.Services.AddDbContext<ApplicationDBContext>(conf => 
        conf.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine,minimumLevel: LogLevel.Error));

// Add the Excel service to the service collection
builder.Services.AddScoped<ExcelService>();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseDefaultFiles();
app.UseStaticFiles();

/// <summary>
/// Endpoint for uploading an Excel file to the server and importing its content into the database.
/// </summary>
/// <param name="service">The Excel service used to handle the import operation.</param>
/// <param name="file">The uploaded Excel file.</param>
/// <returns>A result indicating whether the file was successfully loaded or not.</returns>
app.MapPost("/excel/files/upload", async ([FromServices] ExcelService service, IFormFile file) =>
{
    var isLoaded = await service.ImportIntoDB(file);
    return isLoaded ? Results.Ok("File was loaded") : Results.Problem("File wasn't loaded");
}).DisableAntiforgery();

/// <summary>
/// Endpoint for obtainig a list of uploaded Excel files.
/// </summary>
/// <param name="service">The Excel service used to retrieve the list of files.</param>
/// <returns>A result containing the list of files or an error message if no files were found.</returns>
app.MapGet("/excel/files", async ([FromServices] ExcelService service) =>
{
    var files = await service.GetFiles();
    return files is not null ? Results.Ok(files) : Results.Problem("Files weren't found");
});

/// <summary>
/// Endpoint for obtaining a specific Excel file by its ID.
/// </summary>
/// <param name="service">The Excel service used to retrieve the file.</param>
/// <param name="fileId">The ID of the file to retrieve.</param>
/// <returns>A result containing the file data or an error message if the file was not found.</returns>
app.MapGet("/excel/files/{fileId:int}", async ([FromServices] ExcelService service, int fileId) =>
{
    var file = await service.GetFile(fileId);
    return file is not null ? Results.Ok(file) : Results.Problem("File wasn't found");
});

app.Run();