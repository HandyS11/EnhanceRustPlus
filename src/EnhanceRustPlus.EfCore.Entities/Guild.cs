using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Guild : IEntity
    {
        public ulong Id { get; set; }

        public Category Category { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}
