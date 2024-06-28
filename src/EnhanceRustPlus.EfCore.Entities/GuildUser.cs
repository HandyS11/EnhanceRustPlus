using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(GuildId), nameof(UserId))]
    public class GuildUser
    {
        public Guild Guild { get; set; } = null!;
        public ulong GuildId { get; set; }
        public User User { get; set; } = null!;
        public ulong UserId { get; set; }
    }
}
