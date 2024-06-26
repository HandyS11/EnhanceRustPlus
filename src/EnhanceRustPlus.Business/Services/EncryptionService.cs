using System.Security.Cryptography;
using System.Text;
using EnhanceRustPlus.Business.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EnhanceRustPlus.Business.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] Key;
        private readonly byte[] IV;

        public EncryptionService(IConfiguration configuration)
        {
            var encryptionSettings = configuration.GetSection("encryptionSettings");

            var key = encryptionSettings["key"];
            var iv = encryptionSettings["iv"];

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
                throw new ArgumentNullException("Key and/or IV is missing in the configuration");

            Key = Encoding.UTF8.GetBytes(key);
            IV = Encoding.UTF8.GetBytes(iv);
        }

        public string EncryptString(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string DecryptString(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}
