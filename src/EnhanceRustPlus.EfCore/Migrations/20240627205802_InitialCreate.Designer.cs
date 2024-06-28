﻿// <auto-generated />
using System;
using EnhanceRustPlus.EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EnhanceRustPlus.EfCore.Migrations
{
    [DbContext(typeof(EnhanceRustPlusDbContext))]
    [Migration("20240627205802_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Category", b =>
                {
                    b.Property<ulong>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("HosterId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("HosterId")
                        .IsUnique();

                    b.HasIndex("ServerId")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Channel", b =>
                {
                    b.Property<ulong>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChannelType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ChannelType");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.ChannelType", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("ChannelTypes");

                    b.HasData(
                        new
                        {
                            Name = "INFORMATION"
                        },
                        new
                        {
                            Name = "EVENTS"
                        },
                        new
                        {
                            Name = "TEAM_CHAT"
                        },
                        new
                        {
                            Name = "ALARMS"
                        },
                        new
                        {
                            Name = "SWITCHES"
                        },
                        new
                        {
                            Name = "STORAGE_MONITOR"
                        },
                        new
                        {
                            Name = "ACTIVITY"
                        });
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Credential", b =>
                {
                    b.Property<ulong>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AuthSecret")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GcmAndroidId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GcmSecurityToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PrivateKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("UserCredentials");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Guild", b =>
                {
                    b.Property<ulong>("Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.GuildConfig", b =>
                {
                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("CommandChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("MainCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("RolesId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ServersChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("SettingsChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("UsersChannelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GuildId");

                    b.ToTable("GuildConfigs");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.GuildServer", b =>
                {
                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("TEXT");

                    b.HasKey("GuildId", "ServerId");

                    b.HasIndex("ServerId");

                    b.ToTable("GuildServer");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.GuildUser", b =>
                {
                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GuildId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GuildUser");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Message", b =>
                {
                    b.Property<ulong>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("MessageType");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.MessageType", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("MessageTypes");

                    b.HasData(
                        new
                        {
                            Name = "INFO_MAP"
                        },
                        new
                        {
                            Name = "INFO_INFORMATION"
                        },
                        new
                        {
                            Name = "INFO_EVENT"
                        },
                        new
                        {
                            Name = "INFO_TEAM"
                        },
                        new
                        {
                            Name = "SETTINGS_TITLE"
                        });
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Server", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<string>("Logo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<int>("Port")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.ServerUser", b =>
                {
                    b.Property<Guid>("ServerId")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerToken")
                        .HasColumnType("TEXT");

                    b.HasKey("ServerId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ServerUser");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.User", b =>
                {
                    b.Property<ulong>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<ulong>("SteamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Category", b =>
                {
                    b.HasOne("EnhanceRustPlus.EfCore.Entities.Guild", "Guild")
                        .WithMany("Categories")
                        .HasForeignKey("GuildId");

                    b.HasOne("EnhanceRustPlus.EfCore.Entities.User", "Hoster")
                        .WithOne()
                        .HasForeignKey("EnhanceRustPlus.EfCore.Entities.Category", "HosterId");

                    b.HasOne("EnhanceRustPlus.EfCore.Entities.Server", "Server")
                        .WithOne()
                        .HasForeignKey("EnhanceRustPlus.EfCore.Entities.Category", "ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("Hoster");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Channel", b =>
                {
                    b.HasOne("EnhanceRustPlus.EfCore.Entities.Category", "Category")
                        .WithMany("Channels")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EnhanceRustPlus.EfCore.Entities.ChannelType", null)
                        .WithMany("Channels")
                        .HasForeignKey("ChannelType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Credential", b =>
                {
                    b.HasOne("EnhanceRustPlus.EfCore.Entities.User", "User")
                        .WithOne("Credentials")
                        .HasForeignKey("EnhanceRustPlus.EfCore.Entities.Credential", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.GuildConfig", b =>
                {
                    b.HasOne("EnhanceRustPlus.EfCore.Entities.Guild", "Guild")
                        .WithOne("Config")
                        .HasForeignKey("EnhanceRustPlus.EfCore.Entities.GuildConfig", "GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.GuildServer", b =>
                {
                    b.HasOne("EnhanceRustPlus.EfCore.Entities.Guild", "Guild")
                        .WithMany("GuildServers")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EnhanceRustPlus.EfCore.Entities.Server", "Server")
                        .WithMany("GuildServers")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.GuildUser", b =>
                {
                    b.HasOne("EnhanceRustPlus.EfCore.Entities.Guild", "Guild")
                        .WithMany("GuildUsers")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EnhanceRustPlus.EfCore.Entities.User", "User")
                        .WithMany("GuildUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Message", b =>
                {
                    b.HasOne("EnhanceRustPlus.EfCore.Entities.Channel", "Channel")
                        .WithMany("Messages")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EnhanceRustPlus.EfCore.Entities.MessageType", null)
                        .WithMany("Messages")
                        .HasForeignKey("MessageType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.ServerUser", b =>
                {
                    b.HasOne("EnhanceRustPlus.EfCore.Entities.Server", "Server")
                        .WithMany("ServerUsers")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EnhanceRustPlus.EfCore.Entities.User", "User")
                        .WithMany("ServerUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Server");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Category", b =>
                {
                    b.Navigation("Channels");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Channel", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.ChannelType", b =>
                {
                    b.Navigation("Channels");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Guild", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Config")
                        .IsRequired();

                    b.Navigation("GuildServers");

                    b.Navigation("GuildUsers");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.MessageType", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.Server", b =>
                {
                    b.Navigation("GuildServers");

                    b.Navigation("ServerUsers");
                });

            modelBuilder.Entity("EnhanceRustPlus.EfCore.Entities.User", b =>
                {
                    b.Navigation("Credentials")
                        .IsRequired();

                    b.Navigation("GuildUsers");

                    b.Navigation("ServerUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
