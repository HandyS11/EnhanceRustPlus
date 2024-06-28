using Discord;
using Discord.WebSocket;

using EnhanceRustPlus.Business.Extensions;
using EnhanceRustPlus.Business.Helpers;
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

            var oldGuild = _guildRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == guildId);
            if (oldGuild != null) logger.LogAndThrowBusinessException(Constants.GuildAlreadyExistsInDatabase);

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

            try
            {
                await _guildRepo.AddAsync(guild);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException(Constants.CannotSaveTransactionToDatabase, e);
            }

            logger.LogExitingMethod();
        }

        /// <summary>
        /// Creates a role in the specified guild.
        /// </summary>
        /// <param name="guildId">The ID of the guild.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>The ID of the created role.</returns>
        private async Task<ulong> CreateRoleAsync(ulong guildId, string roleName)
        {
            var guild = client.GetGuild(guildId);
            var role = await guild.CreateRoleAsync(roleName);

            if (role == null) logger.LogAndThrowBusinessException(Constants.RoleCannotBeCreateInDiscord);

            return role!.Id;
        }

        /// <summary>
        /// Creates a category in the specified guild.
        /// </summary>
        /// <param name="guildId">The ID of the guild.</param>
        /// <param name="categoryName">The name of the category.</param>
        /// <param name="roleId">The ID of the role. (optional)</param>
        /// <returns>The ID of the created category.</returns>
        private async Task<ulong> CreateCategoryAsync(ulong guildId, string categoryName, ulong roleId = 0)
        {
            var guild = client.GetGuild(guildId);

            var category = await guild.CreateCategoryChannelAsync(categoryName, properties =>
            {
                properties.Position = 0;

                if (roleId is not 0)
                {
                    properties.PermissionOverwrites = new List<Overwrite>
                    {
                            new (guild.EveryoneRole.Id, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Deny)),
                            new (roleId, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Allow))
                    };
                }
            });

            if (category == null) logger.LogAndThrowBusinessException(Constants.CategoryCannotBeCreateInDiscord);

            return category!.Id;
        }

        /// <summary>
        /// Creates basic channels in the specified guild.
        /// </summary>
        /// <param name="guildId">The ID of the guild.</param>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>A list of IDs of the created channels.</returns>
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
        /// <param name="serverId">The ID of the server.</param>
        /// <param name="roleName">The name of the role. (optional)</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SetupCategory(ulong guildId, string categoryName, Guid serverId, string? roleName = null)
        {
            logger.LogEnteringMethod();

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

            try
            {
                await _categoryRepo.AddAsync(category);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException(Constants.CannotSaveTransactionToDatabase, e);
            }

            logger.LogExitingMethod();
        }

        /// <summary>
        /// Creates channels in the specified guild.
        /// </summary>
        /// <param name="guildId">The ID of the guild.</param>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>A dictionary containing the IDs of the created channels and their corresponding channel types.</returns>
        private async Task<Dictionary<ulong, ChannelTypes>> CreateChannelsAsync(ulong guildId, ulong categoryId)
        {
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

            return channels;
        }
    }
}
