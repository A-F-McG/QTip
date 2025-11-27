using Microsoft.AspNetCore.Http.HttpResults;
using QTipBackend.PiiDetection;
using QTipBackend.src;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var connectionString = "Data Source=data/pii.db";
Database.Initialise(connectionString);

app.MapGet("/get", () => "Hello World!");

app.MapPost("/post", async (string text) =>
{
    if (string.IsNullOrWhiteSpace(text))
    {
        return Results.BadRequest("did not receive text data");
    }

    var emailDetector = new EmailDetector();
    var emails = emailDetector.Detect(text);

    Console.WriteLine(emails);
    return Results.Ok("results ok");
});


app.Run();
