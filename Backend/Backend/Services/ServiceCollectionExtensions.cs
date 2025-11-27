
using Backend.Database;
using Backend.services.databaseOperations;
using Backend.services.encryption;
using Backend.services.PiiDetection;
using Backend.services.PiiTokenisation;
using Backend.Services.piiDetection;
using Microsoft.EntityFrameworkCore;

namespace Backend.services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddDbContext<SqliteDbContext>(options =>
                options.UseSqlite("Data Source=data/pii.db"));
            services.AddScoped<Tokenisation>();
            services.AddScoped<DatabaseOperations>();
            services.AddScoped<IPiiDetector, EmailDetector>();
            services.AddScoped<IPiiDetector, PhoneDetector>();

            // NB - obviously wouldn't be hardcoded keys in a real codebase!
            // Would possibly be stored in Azure Key Vault or similar
            byte[] hardCodedKey = [
                    1, 2, 3, 4, 5, 6, 7, 8,
                    9, 10, 11, 12, 13, 14, 15, 16,
                    17, 18, 19, 20, 21, 22, 23, 24,
                    25, 26, 27, 28, 29, 30, 31, 32
                ];
            services.AddSingleton(new Encryptor(hardCodedKey));

            return services;
        }
    }
}

