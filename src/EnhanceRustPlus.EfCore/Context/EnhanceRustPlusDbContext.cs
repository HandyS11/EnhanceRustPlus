using EnhanceRustPlus.Business.Models.Enums;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Context
{
    public class EnhanceRustPlusDbContext : DbContext
    {
        public DbSet<Category> Guilds { get; set; }
        public DbSet<Channel> Channels { get; set; }

        public DbSet<ChannelType> ChannelTypes { get; set; }

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
                e.HasOne<Category>().WithOne(x => x.Guild);
                e.HasOne<Role>().WithOne(x => x.Guild);
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.HasMany(x => x.Channels).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId);
            });

            modelBuilder.Entity<Channel>(e =>
            {
                e.HasMany(x => x.Messages).WithOne(x => x.Channel).HasForeignKey(x => x.ChannelId);
                e.HasOne<ChannelType>().WithMany(x => x.Channels).HasForeignKey(x => x.ChannelType);
            });

            modelBuilder.Entity<Message>(e =>
            {
                e.HasOne<MessageType>().WithMany(x => x.Messages).HasForeignKey(x => x.MessageType);
            });

            /* Enums */
            modelBuilder.Entity<ChannelType>().HasData(
                EnumExtension.GetEnumAsArrayOutputs<ChannelTypes, ChannelType>(x => new ChannelType { Name = x })
            );

            modelBuilder.Entity<MessageType>().HasData(
                EnumExtension.GetEnumAsArrayOutputs<MessageTypes, MessageType>(x => new MessageType { Name = x })
            );
        }
    }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
}
