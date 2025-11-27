using Backend.models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Database
{
    public class SqliteDbContext: DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options): base(options) { }
        public DbSet<TokenisedSubmission> TokenisedSubmissions => Set<TokenisedSubmission>();
        public DbSet<PiiClassificationVaultEntry> PiiClassificationVault => Set<PiiClassificationVaultEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PiiClassificationVaultEntry>()
                .HasIndex(p => new { p.EncryptedPii, p.TokenizedPii })
                .IsUnique();
        }
    }
}
