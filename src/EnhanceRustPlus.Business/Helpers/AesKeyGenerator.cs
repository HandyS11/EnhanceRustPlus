using System.Security.Cryptography;

namespace EnhanceRustPlus.Business.Helpers
{
    public static class AesKeyGenerator
    {
        public static byte[] GenerateKey(int keySize = 256)
        {
            byte[] key = new byte[keySize / 8]; // 256 bits for AES-256, 128 bits for AES-128
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }

        public static byte[] GenerateIV()
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateIV();
                return aesAlg.IV;
            }
        }
    }
}
