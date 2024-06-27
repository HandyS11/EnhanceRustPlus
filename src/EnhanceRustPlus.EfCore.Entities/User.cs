﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class User : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        public ulong SteamId { get; set; }
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        public Credential Credentials { get; set; } = null!;

        public ICollection<Guild> Guilds { get; set; } = null!;
        public ICollection<Server> Servers { get; set; } = null!;

        public ICollection<ServerUser> ServerUsers { get; set; } = null!;
        public ICollection<GuildUser> GuildUsers { get; set; } = null!;

        public ICollection<Guild> HosterIn { get; set; } = null!;
    }
}
