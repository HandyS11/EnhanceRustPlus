using System.Security.Cryptography;
using System.Text;
using EnhanceRustPlus.Business.Helpers;
using EnhanceRustPlus.Business.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EnhanceRustPlus.Business.Services
{
    /// <summary>
    /// Provides encryption and decryption services using AES algorithm.
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        /// <exception cref="ArgumentNullException">Thrown when Key and/or IV is missing in the configuration.</exception>
        public EncryptionService(IConfiguration configuration)
        {
            var encryptionSettings = configuration.GetSection("encryptionSettings");

            var key = encryptionSettings["key"];
            var iv = encryptionSettings["iv"];

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
                throw new ArgumentNullException(nameof(configuration), Constants.KeyAndOrIvMissingInConfiguration);

            _key = Encoding.UTF8.GetBytes(key);
            _iv = Encoding.UTF8.GetBytes(iv);
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text to encrypt.</param>
        /// <returns>The encrypted cipher text.</returns>
        public string EncryptString(string plainText)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = _key;
            aesAlg.IV = _iv;

            using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using var swEncrypt = new StreamWriter(csEncrypt);
            {
                swEncrypt.Write(plainText);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text to decrypt.</param>
        /// <returns>The decrypted plain text.</returns>
        public string DecryptString(string cipherText)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = _key;
            aesAlg.IV = _iv;

            using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
