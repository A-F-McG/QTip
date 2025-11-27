using System.Security.Cryptography;

namespace Backend.services.encryption
{
    public class Encryptor
    {
        private readonly byte[] _key;
        public Encryptor(byte[] key)
        {
            _key = key;
        }

        public (byte[] encryptedText, byte[] iv) Encrypt(string text)
        {
            var aes = Aes.Create();

            aes.Key = _key;
            aes.GenerateIV();

            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using (StreamWriter StreamWriter = new(cryptoStream))
            {
                StreamWriter.Write(text);
            }

            var encodedText = memoryStream.ToArray();

            return (encodedText, aes.IV);
        }
    }
}
