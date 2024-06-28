namespace EnhanceRustPlus.Business.Interfaces
{
    public interface IEncryptionService
    {
        string EncryptString(string plainText);
        string DecryptString(string cipherText);
    }
}
