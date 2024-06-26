using Discord.Interactions;

namespace EnhanceRustPlus.Business.Parameters
{
    public class CredentialsParameter
    {
        public string GcmAndroidId { get; set; } = null!;
        public string GcmSecurityToken { get; set; } = null!;
        public string PrivateKey { get; set; } = null!;
        public string PublicKey { get; set; } = null!;
        public string AuthSecret { get; set; } = null!;

        [ComplexParameterCtor]
        public CredentialsParameter(string gcmAndroidId, string gcmSecurityToken, string privateKey, string publicKey, string authSecret)
        {
            GcmAndroidId = gcmAndroidId;
            GcmSecurityToken = gcmSecurityToken;
            PrivateKey = privateKey;
            PublicKey = publicKey;
            AuthSecret = authSecret;
        }
    }
}
