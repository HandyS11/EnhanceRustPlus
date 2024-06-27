using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Server
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        [MaxLength(15)]
        public string Ip { get; set; } = null!;
        public int Port { get; set; }
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public string? Image { get; set; }
        public string? Url { get; set; }

        public ICollection<Guild> Guilds { get; set; } = null!;
        public ICollection<User> Users { get; set; } = null!;

        public ICollection<GuildServer> GuildServers { get; set; } = null!;
        public ICollection<ServerUser> ServerUsers { get; set; } = null!;
    }
}
