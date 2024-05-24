using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Guild : IEntity
    {
        public ulong Id { get; set; }
        public ulong CategoryId { get; set; }
        public ulong? RoleId { get; set; }

        public ICollection<Channel> Channels { get; set; } = null!;
    }
}
