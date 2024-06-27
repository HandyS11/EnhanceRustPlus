using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Guild : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }

        public Category Category { get; set; } = null!;
        public Role Role { get; set; } = null!;

        public User? Hoster { get; set; }
        public ulong? HosterId { get; set; }

        public Server? Server { get; set; }
        public Guid? ServerId { get; set; }

        public ICollection<User> Users { get; set; } = null!;
        public ICollection<Server> Servers { get; set; } = null!;

        public ICollection<GuildUser> GuildUsers { get; set; } = null!;
        public ICollection<GuildServer> GuildServers { get; set; } = null!;
    }
}
