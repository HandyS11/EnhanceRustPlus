using System.ComponentModel.DataAnnotations;
using EnhanceRustPlus.Business.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Name))]
    public class MessageType
    {
        [MaxLength(30)]
        public MessageTypes Name { get; set; }

        public ICollection<Message> Messages { get; set; } = null!;
    }
}
