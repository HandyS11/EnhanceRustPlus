using System.ComponentModel.DataAnnotations;
using EnhanceRustPlus.Business.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Name))]
    public class ChannelType
    {
        [MaxLength(30)]
        public ChannelTypes Name { get; set; }

        public ICollection<Channel> Channels { get; set; } = null!;
    }
}
