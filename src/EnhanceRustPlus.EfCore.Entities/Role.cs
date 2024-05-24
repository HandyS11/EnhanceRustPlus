using System.ComponentModel.DataAnnotations;
using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Role : IEntity
    {
        public ulong Id { get; set; }
        [MaxLength(30)]
        public string? Name { get; set; }

        public Guild Guild { get; set; } = null!;
    }
}
