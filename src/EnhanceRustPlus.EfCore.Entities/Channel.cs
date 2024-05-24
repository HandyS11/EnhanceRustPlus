﻿using System.ComponentModel.DataAnnotations;
using EnhanceRustPlus.EfCore.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Channel : IEntity
    {
        public ulong Id { get; set; }
        [MaxLength(30)]
        public string? ChannelType { get; set; }

        public Category Category { get; set; } = null!;
        public ulong CategoryId { get; set; }

        public ICollection<Message> Messages { get; set; } = null!;
    }
}
