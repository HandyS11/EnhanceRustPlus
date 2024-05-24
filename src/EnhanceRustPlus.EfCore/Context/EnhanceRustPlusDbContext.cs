using EnhanceRustPlus.Business.Models.Enums;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Entities.Types;
using EnhanceRustPlus.EfCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Context
{
    public class EnhanceRustPlusDbContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }

        public EnhanceRustPlusDbContext() { }

        public EnhanceRustPlusDbContext(DbContextOptions<EnhanceRustPlusDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=EnhanceRustPlus.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>(e =>
            {
                e.HasMany(x => x.Channels).WithOne(x => x.Guild).HasForeignKey(x => x.GuildId);
            });

            modelBuilder.Entity<Channel>(e =>
            {
                e.HasOne<ChannelType>().WithMany(x => x.Channels).HasForeignKey(x => x.ChannelName);
            });

            /* Enums */
            modelBuilder.Entity<ChannelType>().HasData(
                EnumExtension.GetEnumAsArrayOutputs<ChannelTypes, ChannelType>(x => new ChannelType { Name = x })
            );
        }
    }
}
