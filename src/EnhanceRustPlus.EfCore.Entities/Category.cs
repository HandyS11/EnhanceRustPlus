using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Category : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        [MaxLength(30)]
        public string? Name { get; set; }

        public Guild Guild { get; set; } = null!;
        public ulong GuildId { get; set; }

        public ICollection<Channel> Channels { get; set; } = null!;
    }
}