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

        public DbSet<User> Users { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Server> Servers { get; set; }

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
                e.HasOne(g => g.Category).WithOne(c => c.Guild).HasForeignKey<Category>(c => c.GuildId).IsRequired();
                e.HasOne(g => g.Role).WithOne(r => r.Guild).HasForeignKey<Role>(r => r.GuildId).IsRequired();

                e.HasMany(g => g.Users)
                    .WithMany(u => u.Guilds)
                    .UsingEntity<GuildUser>(
                        j => j.HasOne(gu => gu.User).WithMany(u => u.GuildUsers).HasForeignKey(gu => gu.UserId),
                        j => j.HasOne(gu => gu.Guild).WithMany(g => g.GuildUsers).HasForeignKey(gu => gu.GuildId));

                e.HasMany(g => g.Servers)
                    .WithMany(s => s.Guilds)
                    .UsingEntity<GuildServer>(
                        j => j.HasOne(gs => gs.Server).WithMany(s => s.GuildServers).HasForeignKey(gs => gs.ServerId),
                        j => j.HasOne(gs => gs.Guild).WithMany(g => g.GuildServers).HasForeignKey(gs => gs.GuildId));

                e.HasOne(g => g.Hoster).WithMany(g => g.HosterIn).HasForeignKey(g => g.HosterId).IsRequired(false);
                e.HasOne(g => g.Server).WithMany(g => g.ServerIn).HasForeignKey(g => g.ServerId).IsRequired(false);
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

            modelBuilder.Entity<User>(e =>
            {
                e.HasOne(u => u.Credentials).WithOne(c => c.User).HasForeignKey<Credential>(c => c.UserId);
                e.HasMany(u => u.Servers)
                    .WithMany(s => s.Users)
                    .UsingEntity<ServerUser>(
                        j => j.HasOne(su => su.Server).WithMany(s => s.ServerUsers).HasForeignKey(su => su.ServerId),
                        j => j.HasOne(su => su.User).WithMany(u => u.ServerUsers).HasForeignKey(su => su.UserId)
                );
            });

            /* Enums tables */
            modelBuilder.Entity<ChannelType>(e => e.Property(c => c.Name).HasConversion<string>());
            modelBuilder.Entity<MessageType>(e => e.Property(c => c.Name).HasConversion<string>());

            /* Enums mapping */
            modelBuilder.Entity<ChannelType>().HasData(EnumExtension.GetEnumAsArray<ChannelTypes>().Select(x => new ChannelType { Name = x }));
            modelBuilder.Entity<MessageType>().HasData(EnumExtension.GetEnumAsArray<MessageTypes>().Select(x => new MessageType { Name = x }));
        }
    }
}
