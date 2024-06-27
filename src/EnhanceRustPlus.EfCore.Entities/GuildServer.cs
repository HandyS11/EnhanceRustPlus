using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(GuildId), nameof(ServerId))]
    public class GuildServer
    {
        public Guild Guild { get; set; } = null!;
        public ulong GuildId { get; set; }
        public Server Server { get; set; } = null!;
        public Guid ServerId { get; set; }
    }
}
