using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Name))]
    public class MessageType
    {
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        public ICollection<Message> Messages { get; set; } = null!;
    }
}
