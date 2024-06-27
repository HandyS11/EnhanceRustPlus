using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(UserId))]
    public class Credential
    {
        public string GcmAndroidId { get; set; } = null!;
        public string GcmSecurityToken { get; set; } = null!;
        public string PrivateKey { get; set; } = null!;
        public string PublicKey { get; set; } = null!;
        public string AuthSecret { get; set; } = null!;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
