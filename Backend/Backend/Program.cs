using Backend.Database;
using Backend.DTOs;
using Backend.models;
using Backend.services;
using Backend.services.databaseOperations;
using Backend.services.PiiDetection;
using Backend.services.PiiTokenisation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices();
var app = builder.Build();
app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
    db.Database.EnsureCreated(); 
}

app.MapGet("/getEmailCount", (DatabaseOperations db) => db.GetPiiCount());

app.MapPost("/sendSubmission", (IEnumerable<IPiiDetector> piiDetectors, DatabaseOperations submissions, Tokenisation tokenisation, Submission submission) =>
{
    var submissionText = submission.Text;

    if (string.IsNullOrWhiteSpace(submissionText))
    {
        return Results.BadRequest("did not receive text data in submission");
    }

    try
    {
        var allPiiTokens = new Dictionary<string, string>();

        
        foreach (var piiDetector in piiDetectors) {
            var pii = piiDetector.DetectDistinct(submissionText);
            if (pii.Count > 0)
            {
                var piiToTokens = tokenisation.PiiToToken(pii);
                allPiiTokens = allPiiTokens.Concat(piiToTokens).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                submissions.InsertPiiClassifications(pii, piiToTokens, piiDetector.Type);
            }
        }

        submissions.InsertTokenisedSubmission(submissionText, allPiiTokens);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }

    return Results.Ok();
});


app.Run();
