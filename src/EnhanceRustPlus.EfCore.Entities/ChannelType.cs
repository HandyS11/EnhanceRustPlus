using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Name))]
    public class ChannelType
    {
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        public ICollection<Channel> Channels { get; set; } = null!;
    }
}
