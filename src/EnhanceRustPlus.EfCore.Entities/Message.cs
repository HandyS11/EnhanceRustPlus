using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnhanceRustPlus.Business.Models.Enums;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Message : IDiscordEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        [MaxLength(30)]
        public MessageTypes MessageType { get; set; }

        public Channel Channel { get; set; } = null!;
        public ulong ChannelId { get; set; }
    }
}
