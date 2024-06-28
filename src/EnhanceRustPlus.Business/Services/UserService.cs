using EnhanceRustPlus.Business.Exceptions;
using EnhanceRustPlus.Business.Extensions;
using EnhanceRustPlus.Business.Helpers;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnhanceRustPlus.Business.Services
{
    /// <summary>
    /// Represents a service for managing users.
    /// </summary>
    public class UserService(ILogger<UserService> logger, IUnitOfWork uow) : IUserService
    {
        private readonly IRepositoryManager<Guild> _guildRepo = uow.GetRepository<Guild>();
        private readonly IRepositoryManager<GuildUser> _guildUserRepo = uow.GetRepository<GuildUser>();
        private readonly IRepositoryManager<User> _userRepo = uow.GetRepository<User>();

        /// <summary>
        /// Creates a new user with the specified details.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        /// <param name="steamId">The Steam ID of the user.</param>
        /// <param name="name">The name of the user.</param>
        /// <param name="avatar">The avatar of the user.</param>
        public async Task CreateUser(ulong discordId, ulong steamId, string name, string avatar)
        {
            logger.LogEnteringMethod();

            var oldUser = _userRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == discordId);
            if (oldUser != null) throw new BusinessException(Constants.UserAlreadyExistsInDatabase);

            var user = new User
            {
                Id = discordId,
                SteamId = steamId,
                Name = name,
                Avatar = avatar
            };

            try
            {
                await _userRepo.AddAsync(user);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException(Constants.CannotSaveTransactionToDatabase, e);
            }

            logger.LogExitingMethod();
        }

        /// <summary>
        /// Updates the details of an existing user.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        /// <param name="steamId">The new Steam ID of the user.</param>
        /// <param name="name">The new name of the user.</param>
        /// <param name="avatar">The new avatar of the user.</param>
        public async Task UpdateUser(ulong discordId, ulong steamId = 0, string? name = null, string? avatar = null)
        {
            logger.LogEnteringMethod();

            var user = _userRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == discordId);
            if (user == null) throw new BusinessException(Constants.UserNotFoundInDatabase);

            if (steamId != 0) user.SteamId = steamId;
            if (name != null) user.Name = name;
            if (avatar != null) user.Avatar = avatar;

            try
            {
                _userRepo.Update(user);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException(Constants.CannotSaveTransactionToDatabase, e);
            }

            logger.LogExitingMethod();
        }


        /// <summary>
        /// Registers a user to a guild.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        /// <param name="guildId">The ID of the guild.</param>
        public async Task RegisterUser(ulong discordId, ulong guildId)
        {
            logger.LogEnteringMethod();

            var user = _userRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == discordId);
            if (user == null) throw new BusinessException(Constants.UserNotFoundInDatabase);

            var guild = _guildRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == guildId);
            if (guild == null) throw new BusinessException(Constants.GuildNotFoundInDatabase);

            var userGuild = _guildUserRepo.GetAsIQueryable().FirstOrDefault(x => x.GuildId == guildId && x.UserId == discordId);
            if (userGuild != null) throw new BusinessException(Constants.UserAlreadyRegisteredInGuild);

            var guildUser = new GuildUser
            {
                GuildId = guildId,
                UserId = discordId
            };

            try
            {
                await _guildUserRepo.AddAsync(guildUser);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException(Constants.CannotSaveTransactionToDatabase, e);
            }

            logger.LogExitingMethod();
        }

        /// <summary>
        /// Unregisters a user from a guild.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        /// <param name="guildId">The ID of the guild.</param>
        public async Task UnregisterUser(ulong discordId, ulong guildId)
        {
            logger.LogEnteringMethod();

            var user = _userRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == discordId);
            if (user == null) throw new BusinessException(Constants.UserNotFoundInDatabase);

            var userGuild = _guildUserRepo.GetAsIQueryable().FirstOrDefault(x => x.GuildId == guildId && x.UserId == discordId);
            if (userGuild == null) throw new BusinessException(Constants.UserNotRegisteredInGuild);

            try
            {
                _guildUserRepo.Delete(userGuild);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException(Constants.CannotSaveTransactionToDatabase, e);
            }

            logger.LogExitingMethod();
        }

        /// <summary>
        /// Deletes a user and removes them from all guilds.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        public async Task DeleteUser(ulong discordId)
        {
            logger.LogEnteringMethod();

            var user = _userRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == discordId);
            if (user == null) throw new BusinessException(Constants.UserNotFoundInDatabase);

            var userGuilds = _guildUserRepo.GetAsIQueryable().Where(x => x.UserId == discordId).ToList();

            try
            {
                userGuilds.ForEach(x => _guildUserRepo.Delete(x));

                _userRepo.Delete(user);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException(Constants.CannotSaveTransactionToDatabase, e);
            }

            logger.LogExitingMethod();
        }
    }
}
