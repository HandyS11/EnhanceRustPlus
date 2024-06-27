﻿using Discord;
using Discord.WebSocket;

using EnhanceRustPlus.Business.Exceptions;
using EnhanceRustPlus.Business.Extensions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.Business.Models.Enums;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Interfaces;

using Microsoft.Extensions.Logging;

namespace EnhanceRustPlus.Business.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SetupService(DiscordSocketClient client, ILogger<SetupService> logger, IUnitOfWork uow) : ISetupService
    {
        private readonly IRepositoryManager<Guild> _guildRepo = uow.GetRepository<Guild>();
        private readonly IRepositoryManager<Category> _categoryRepo = uow.GetRepository<Category>();

        /// <summary>
        /// Sets up Discord for a guild by creating a role, a category, and basic channels.
        /// </summary>
        /// <param name="guildId">The ID of the guild.</param>
        /// <param name="categoryName">The name of the category.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SetupDiscord(ulong guildId, string categoryName, string roleName)
        {
            logger.LogEnteringMethod();

            try
            {
                var roleId = await CreateRoleAsync(guildId, roleName);
                var mainCategoryId = await CreateCategoryAsync(guildId, categoryName, roleId);
                var basicChannels = await CreateBasicsChannelsAsync(guildId, mainCategoryId);

                var guildConfig = new GuildConfig
                {
                    GuildId = guildId,
                    RolesId = roleId,
                    MainCategoryId = mainCategoryId,
                    ServersChannelId = basicChannels[0],
                    UsersChannelId = basicChannels[1],
                    SettingsChannelId = basicChannels[2],
                    CommandChannelId = basicChannels[3]
                };

                var guild = new Guild
                {
                    Id = guildId,
                    Config = guildConfig,
                };

                await _guildRepo.AddAsync(guild);
                await uow.SaveAsync();
            }
            catch (BusinessException e)
            {
                logger.LogError(e, "Error setting up Discord");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unknown Error");
            }

            logger.LogExitingMethod();
        }

        private async Task<ulong> CreateRoleAsync(ulong guildId, string roleName)
        {
            var guild = client.GetGuild(guildId);
            var role = await guild.CreateRoleAsync(roleName);

            if (role == null) logger.LogAndThrowBusinessException("Role could not be created");

            return role!.Id;
        }
        private async Task<ulong> CreateCategoryAsync(ulong guildId, string categoryName, ulong roleId = 0)
        {
            var guild = client.GetGuild(guildId);

            var category = await guild.CreateCategoryChannelAsync(categoryName, properties =>
            {
                List<Overwrite> overwrites = [
                    new(guild.EveryoneRole.Id, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Deny)),
                ];

                if (roleId is not 0)
                    overwrites.Add(new((ulong)roleId, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Allow)));

                properties.Position = 0;
                properties.PermissionOverwrites = overwrites;
            });

            if (category == null) logger.LogAndThrowBusinessException("Category could not be created");

            return category!.Id;
        }

        private async Task<List<ulong>> CreateBasicsChannelsAsync(ulong guildId, ulong categoryId)
        {
            var guild = client.GetGuild(guildId);

            var channels = new List<ulong>();
            List<string> channelNames = [
                "servers",
                "users",
                "settings",
                "commands"
            ];

            foreach (var name in channelNames)
            {
                var channel = await guild.CreateTextChannelAsync(name, properties =>
                {
                    properties.CategoryId = categoryId;
                });

                channels.Add(channel.Id);
            }

            return channels;
        }

        /// <summary>
        /// Sets up a category for a guild by creating a role, a category, and channels.
        /// </summary>
        /// <param name="guildId">The ID of the guild.</param>
        /// <param name="categoryName">The name of the category.</param>
        /// <param name="roleName">The name of the role. (optional)</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SetupCategory(ulong guildId, string categoryName, Guid serverId, string? roleName = null)
        {
            logger.LogEnteringMethod();

            try
            {
                var roleId = roleName is not null ? await CreateRoleAsync(guildId, roleName) : 0;
                var mainCategoryId = await CreateCategoryAsync(guildId, categoryName, roleId);
                var channels = await CreateChannelsAsync(guildId, mainCategoryId);

                var category = new Category
                {
                    GuildId = guildId,
                    RoleId = roleId == 0 ? null : roleId,
                    ServerId = serverId,
                    Channels = channels.Select(x => new Channel
                    {
                        Id = x.Key,
                        ChannelType = x.Value
                    }).ToList()
                };

                await _categoryRepo.AddAsync(category);
                await uow.SaveAsync();
            }
            catch (BusinessException e)
            {
                logger.LogError(e, "Error setting up Categories");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unknown Error");
            }

            logger.LogExitingMethod();
        }

        private async Task<Dictionary<ulong, ChannelTypes>> CreateChannelsAsync(ulong guildId, ulong categoryId)
        {
            logger.LogEnteringMethod();

            var guild = client.GetGuild(guildId);

            var names = Enum.GetNames(typeof(ChannelTypes));
            var channelNames = names.Select(x => x.ToLower().Replace('_', '-')).ToList();

            var channels = new Dictionary<ulong, ChannelTypes>();

            foreach (var name in channelNames)
            {
                var channel = await guild.CreateTextChannelAsync(name, properties =>
                {
                    properties.CategoryId = categoryId;
                });

                Enum.TryParse<ChannelTypes>(name, true, out var enumValue);
                channels.Add(channel.Id, enumValue);
            }

            logger.LogExitingMethod();

            return channels;
        }
    }
}
