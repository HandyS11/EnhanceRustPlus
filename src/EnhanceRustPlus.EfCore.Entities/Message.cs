using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Message : IEntity
    {
        public ulong Id { get; set; }
        [MaxLength(30)]
        public string? MessageType { get; set; }

        public Channel Channel { get; set; } = null!;
        public ulong ChannelId { get; set; }
    }
}
