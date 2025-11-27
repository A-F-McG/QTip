using Backend.Database;
using Backend.models;
using Backend.services.encryption;
using Backend.services.PiiTokenisation;
using Microsoft.EntityFrameworkCore;

namespace Backend.services.databaseOperations
{
    public class DatabaseOperations(Encryptor encryptor, SqliteDbContext db, Tokenisation tokenisation)
    {
        private readonly Encryptor _encryptor = encryptor;
        private readonly SqliteDbContext _db = db;
        private readonly Tokenisation _tokenisation = tokenisation;

        public async void InsertPiiClassifications(List<string> piis, Dictionary<string, string> piiToToken, string piiType)
        {
            foreach (var pii in piis)
            {
                var (encryptedText, iv) = _encryptor.Encrypt(pii);

                _db.PiiClassificationVault.Add(new PiiClassificationVaultEntry
                {
                    EncryptedPii = encryptedText,
                    Iv = iv,
                    TokenizedPii = piiToToken[pii],
                    Type = piiType
                });
            }

            await _db.SaveChangesAsync();
        }

        public async void InsertTokenisedSubmission(string plainText, Dictionary<string, string> piiToToken)
        {
            _db.TokenisedSubmissions.Add(new TokenisedSubmission
            {
                TokenisedText = _tokenisation.BuildTokenisedText(plainText, piiToToken)
            });

            await _db.SaveChangesAsync();
        }

        public async Task<int> GetPiiCount()
        {
            return await _db.PiiClassificationVault.CountAsync();
        }

    }
}
