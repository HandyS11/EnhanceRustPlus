using Discord;
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
        private readonly IRepositoryManager<Guild> _guildRepo = uow.GetRepositoryManager<Guild>();

        public async Task<bool> SetupDiscord(ulong guildId, string roleName, string categoryName)
        {
            logger.LogEnteringMethod();

            try
            {
                var roleId = await CreateRoleAsync(guildId, roleName);
                var categoryId = await CreateCategoryAsync(guildId, roleId, categoryName);
                var channels = await CreateChannelsAsync(guildId, categoryId);

                var guild = new Guild
                {
                    Id = guildId,
                    Role = new Role
                    {
                        Id = roleId,
                        Name = roleName
                    },
                    Category = new Category
                    {
                        Id = categoryId,
                        Name = categoryName,
                        Channels = channels.Select(x => new Channel
                        {
                            Id = x.Key,
                            ChannelType = x.Value
                        }).ToList()
                    }
                };

                await _guildRepo.AddAsync(guild);
                await uow.SaveAsync();
            }
            catch (BusinessException e)
            {
                logger.LogError(e, "Error setting up Discord");
                return false;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unknown Error");
                return false;
            }

            logger.LogExitingMethod();

            return true;
        }

        private async Task<ulong> CreateRoleAsync(ulong guildId, string roleName)
        {
            logger.LogEnteringMethod();

            var guild = client.GetGuild(guildId);
            var role = await guild.CreateRoleAsync(roleName);

            if (role == null) logger.LogAndThrowBusinessException("Role could not be created");

            logger.LogExitingMethod();

            return role!.Id;
        }

        private async Task<ulong> CreateCategoryAsync(ulong guildId, ulong roleId, string categoryName)
        {
            logger.LogEnteringMethod();

            var guild = client.GetGuild(guildId);

            var category = await guild.CreateCategoryChannelAsync(categoryName, properties =>
            {
                properties.Position = 0;
                properties.PermissionOverwrites = new List<Overwrite>
                {
                    new(guild.EveryoneRole.Id, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Deny)),
                    new(roleId, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Allow))
                };
            });

            if (category == null) logger.LogAndThrowBusinessException("Category could not be created");

            logger.LogExitingMethod();

            return category!.Id;
        }

        private async Task<Dictionary<ulong, string>> CreateChannelsAsync(ulong guildId, ulong categoryId)
        {
            logger.LogEnteringMethod();

            var guild = client.GetGuild(guildId);

            var names = Enum.GetNames(typeof(ChannelTypes));
            var channelNames = names.Select(x => x.ToLower().Replace('_', '-')).ToList();

            var channels = new Dictionary<ulong, string>();

            foreach (var name in channelNames)
            {
                var channel = await guild.CreateTextChannelAsync(name, properties =>
                {
                    properties.CategoryId = categoryId;
                });
                channels.Add(channel.Id, channel.Name);
            }

            logger.LogExitingMethod();

            return channels;
        }

        private Task<Dictionary<ulong, string>> CreateSettingsMessagesAsync(ulong guildId)
        {
            throw new NotImplementedException();
        }
    }
}
