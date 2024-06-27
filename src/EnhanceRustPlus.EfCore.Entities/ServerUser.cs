using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(ServerId), nameof(UserId))]
    public class ServerUser
    {
        public Server Server { get; set; } = null!;
        public Guid ServerId { get; set; }
        public User User { get; set; } = null!;
        public ulong UserId { get; set; }

        public string? PlayerToken { get; set; }
    }
}
