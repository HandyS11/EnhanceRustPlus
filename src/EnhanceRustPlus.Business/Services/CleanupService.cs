using Discord.WebSocket;

using EnhanceRustPlus.Business.Extensions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Interfaces;
using Microsoft.EntityFrameworkCore;
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
                    .Include(x => x.Category)
                        .ThenInclude(x => x.Channels)
                    .Include(x => x.Role)
                    .FirstOrDefault(x => x.Id == guildId);

                if (guildDatabase == null) logger.LogAndThrowBusinessException("Guild not found in database");

                var guildDiscord = client.GetGuild(guildId);

                if (guildDatabase?.Role == null) logger.LogAndThrowBusinessException("Role not found in database");

                var role = guildDiscord.GetRole(guildDatabase!.Role.Id);
                role?.DeleteAsync();

                if (guildDatabase?.Category == null) logger.LogAndThrowBusinessException("Category not found in database");

                var channels = guildDatabase!.Category.Channels;
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
