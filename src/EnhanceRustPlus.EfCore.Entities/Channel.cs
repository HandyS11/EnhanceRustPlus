using System.ComponentModel.DataAnnotations;
using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Channel : IEntity
    {
        public ulong Id { get; set; }
        [MaxLength(30)]
        public string? ChannelName { get; set; }

        public Guild Guild { get; set; } = null!;
        public ulong GuildId { get; set; }
    }
}
