using System.ComponentModel.DataAnnotations.Schema;
using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Category : IDiscordEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }

        public ulong? RoleId { get; set; }

        public Guild Guild { get; set; } = null!;
        public ulong GuildId { get; set; }

        public Server Server { get; set; } = null!;
        public Guid ServerId { get; set; }

        public User? Hoster { get; set; }
        public ulong? HosterId { get; set; }

        public ICollection<Channel> Channels { get; set; } = null!;
    }
}