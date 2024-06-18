using EnhanceRustPlus.Business.Models.Enums;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Context
{
    public class EnhanceRustPlusDbContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<ChannelType> ChannelTypes { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }

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
                e.HasOne(g => g.Category)
                    .WithOne(c => c.Guild)
                    .HasForeignKey<Category>(c => c.GuildId)
                    .IsRequired();
                e.HasOne(g => g.Role)
                    .WithOne(r => r.Guild)
                    .HasForeignKey<Role>(r => r.GuildId)
                    .IsRequired();
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.HasMany(x => x.Channels)
                    .WithOne(x => x.Category)
                    .HasForeignKey(x => x.CategoryId);
            });

            modelBuilder.Entity<Channel>(e =>
            {
                e.HasMany(x => x.Messages)
                    .WithOne(x => x.Channel)
                    .HasForeignKey(x => x.ChannelId);
                e.HasOne<ChannelType>()
                    .WithMany(x => x.Channels)
                    .HasForeignKey(x => x.ChannelType);
            });

            modelBuilder.Entity<Message>(e =>
            {
                e.HasOne<MessageType>()
                    .WithMany(x => x.Messages)
                    .HasForeignKey(x => x.MessageType);
            });

            modelBuilder.Entity<ChannelType>(e =>
            {
                e.Property(c => c.Name).HasConversion<string>();
            });

            modelBuilder.Entity<MessageType>(e =>
            {
                e.Property(c => c.Name).HasConversion<string>();
            });

            /* Enums */
            modelBuilder.Entity<ChannelType>().HasData(
                EnumExtension.GetEnumAsArray<ChannelTypes>().Select(x => new ChannelType { Name = x })
            );

            modelBuilder.Entity<MessageType>().HasData(
                EnumExtension.GetEnumAsArray<MessageTypes>().Select(x => new MessageType { Name = x })
            );
        }
    }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
}
