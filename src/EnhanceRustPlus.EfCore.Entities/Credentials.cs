using EnhanceRustPlus.EfCore.Entities.Interfaces;

namespace EnhanceRustPlus.EfCore.Entities
{
    public class Credentials : IEntity
    {
        public ulong Id { get; set; }

        public string GcmAndroidId { get; set; } = null!;
        public string GcmSecurityToken { get; set; } = null!;
        public string PrivateKey { get; set; } = null!;
        public string PublicKey { get; set; } = null!;
        public string AuthSecret { get; set; } = null!;
    }
}
