using Discord.Interactions;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ConvertToPrimaryConstructor

namespace EnhanceRustPlus.Business.Parameters
{
    public class CredentialsParameter
    {
        public ulong GcmAndroidId { get; }
        public ulong GcmSecurityToken { get; }
        public string PrivateKey { get; }
        public string PublicKey { get; }
        public string AuthSecret { get; }

        public CredentialsParameter()
        {
            GcmAndroidId = 0;
            GcmSecurityToken = 0;
            PrivateKey = string.Empty;
            PublicKey = string.Empty;
            AuthSecret = string.Empty;
        }

        [ComplexParameterCtor]
        public CredentialsParameter(ulong gcmAndroidId, ulong gcmSecurityToken, string privateKey, string publicKey, string authSecret)
        {
            GcmAndroidId = gcmAndroidId;
            GcmSecurityToken = gcmSecurityToken;
            PrivateKey = privateKey;
            PublicKey = publicKey;
            AuthSecret = authSecret;
        }
    }
}
