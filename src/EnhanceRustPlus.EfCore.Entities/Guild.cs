using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Guild : IDiscordEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }

        public GuildConfig Config { get; set; } = null!;

        public ICollection<Category> Categories { get; set; } = null!;

        public ICollection<User> Users { get; set; } = null!;
        public ICollection<Server> Servers { get; set; } = null!;

        public ICollection<GuildUser> GuildUsers { get; set; } = null!;
        public ICollection<GuildServer> GuildServers { get; set; } = null!;
    }
}
