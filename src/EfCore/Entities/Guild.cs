using EfCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Guild : IEntity
    {
        public ulong Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ulong RoleId { get; set; }
        public ulong CategoryId { get; set; }
    }
}
