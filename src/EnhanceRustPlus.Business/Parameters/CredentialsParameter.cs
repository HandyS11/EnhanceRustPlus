using Discord.Interactions;

namespace EnhanceRustPlus.Business.Parameters
{
    public class CredentialsParameter
    {
        public string GcmAndroidId { get; }
        public string GcmSecurityToken { get; }
        public string PrivateKey { get; }
        public string PublicKey { get; }
        public string AuthSecret { get; }

        public CredentialsParameter()
        {
            GcmAndroidId = string.Empty;
            GcmSecurityToken = string.Empty;
            PrivateKey = string.Empty;
            PublicKey = string.Empty;
            AuthSecret = string.Empty;
        }

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
