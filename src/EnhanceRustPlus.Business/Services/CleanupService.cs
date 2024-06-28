using Discord.WebSocket;

using EnhanceRustPlus.Business.Extensions;
using EnhanceRustPlus.Business.Helpers;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EnhanceRustPlus.Business.Services
{
    /// <summary>
    /// Service responsible for performing cleanup operations in a guild.
    /// </summary>
    public class CleanupService(DiscordSocketClient client, ILogger<CleanupService> logger, IUnitOfWork uow) : ICleanupService
    {
        private readonly IRepositoryManager<Guild> _guildRepo = uow.GetRepository<Guild>();

        /// <summary>
        /// Performs cleanup operations for the specified guild.
        /// </summary>
        /// <param name="guildId">The ID of the guild to perform cleanup for.</param>
        /// <returns>A task representing the asynchronous cleanup operation.</returns>
        public async Task CleanupAsync(ulong guildId)
        {
            logger.LogEnteringMethod();

            var guildDatabase = _guildRepo.GetAsIQueryable()
                .Include(x => x.Config)
                .Include(x => x.Categories)
                .ThenInclude(x => x.Channels)
                .FirstOrDefault(x => x.Id == guildId);

            if (guildDatabase == null) logger.LogAndThrowBusinessException(Constants.GuildNotFoundInDatabase);

            var guildDiscord = client.GetGuild(guildId);
            if (guildDiscord == null) logger.LogAndThrowBusinessException(Constants.GuildNotFoundInDiscord);

            var role = guildDiscord!.GetRole(guildDatabase!.Config.RolesId);
            role?.DeleteAsync();

            foreach (var category in guildDatabase.Categories)
            {
                var channels = guildDatabase!.Categories?.SelectMany(x => x.Channels).ToList();
                channels?.ForEach(channel =>
                {
                    var channelDiscord = guildDiscord.GetChannel(channel.Id);
                    channelDiscord?.DeleteAsync();
                });

                if (category.RoleId is not null)
                {
                    var categoryRole = guildDiscord.GetRole((ulong)category.RoleId);
                    categoryRole?.DeleteAsync();
                }

                var categoryDiscord = guildDiscord.GetCategoryChannel(category.Id);
                categoryDiscord?.DeleteAsync();
            }

            var serverChannel = guildDiscord.GetChannel(guildDatabase!.Config.ServersChannelId);
            serverChannel?.DeleteAsync();
            var userChannel = guildDiscord.GetChannel(guildDatabase!.Config.UsersChannelId);
            userChannel?.DeleteAsync();
            var settingsChannel = guildDiscord.GetChannel(guildDatabase!.Config.SettingsChannelId);
            settingsChannel?.DeleteAsync();
            var commandChannel = guildDiscord.GetChannel(guildDatabase!.Config.CommandChannelId);
            commandChannel?.DeleteAsync();

            var mainCategory = guildDiscord.GetCategoryChannel(guildDatabase!.Config.MainCategoryId);
            mainCategory?.DeleteAsync();

            try
            {
                await _guildRepo.DeleteAsync(guildDatabase.Id);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException(Constants.CannotSaveTransactionInDatabase, e);
            }

            logger.LogExitingMethod();
        }
    }
}
