using Discord.WebSocket;

using EnhanceRustPlus.Business.Extensions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Interfaces;

using Microsoft.Extensions.Logging;

namespace EnhanceRustPlus.Business.Services
{
    public class CleanupService(DiscordSocketClient client, ILogger<CleanupService> logger, IUnitOfWork uow) : ICleanupService
    {
        private readonly IRepositoryManager<Guild> _guildRepo = uow.GetRepository<Guild>();

        public async Task CleanupAsync(ulong guildId)
        {
            logger.LogEnteringMethod();

            try
            {
                var guildDatabase = _guildRepo.GetAsIQueryable()
                    .FirstOrDefault(x => x.Id == guildId);

                if (guildDatabase == null) logger.LogAndThrowBusinessException("Guild not found in database");

                var guildDiscord = client.GetGuild(guildId);

                var role = guildDiscord.GetRole(guildDatabase!.Role.Id);
                role?.DeleteAsync();

                var channels = guildDatabase.Category.Channels;
                foreach (var channel in channels)
                {
                    var channelDiscord = guildDiscord.GetChannel(channel.Id);
                    channelDiscord?.DeleteAsync();
                }

                var category = guildDiscord.GetCategoryChannel(guildDatabase.Category.Id);
                category?.DeleteAsync();

                await _guildRepo.DeleteAsync(guildDatabase.Id);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException("Error cleaning up Discord", e);
            }

            logger.LogExitingMethod();
        }
    }
}
