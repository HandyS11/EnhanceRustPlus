using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnhanceRustPlus.Business.Models.Enums;
using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Channel : IDiscordEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        [MaxLength(30)]
        public ChannelTypes ChannelType { get; set; }

        public Category Category { get; set; } = null!;
        public ulong CategoryId { get; set; }

        public ICollection<Message> Messages { get; set; } = null!;
    }
}
