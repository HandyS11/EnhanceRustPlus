namespace EnhanceRustPlus.Business.Helpers
{
    public static class Constants
    {
        public const string CannotSaveTransactionInDatabase = nameof(CannotSaveTransactionInDatabase);
        public const string GuildAlreadyExistsInDatabase = nameof(GuildAlreadyExistsInDatabase);
        public const string GuildNotFoundInDatabase = nameof(GuildNotFoundInDatabase);
        public const string UserAlreadyExistsInDatabase = nameof(UserAlreadyExistsInDatabase);
        public const string UserAlreadyRegisteredInGuild = nameof(UserAlreadyRegisteredInGuild);
        public const string UserNotFoundInDatabase = nameof(UserNotFoundInDatabase);
        public const string UserNotRegisteredInGuild = nameof(UserNotRegisteredInGuild);

        public const string GuildNotFoundInDiscord = nameof(GuildNotFoundInDiscord);
        public const string CategoryCannotBeCreateInDiscord = nameof(CategoryCannotBeCreateInDiscord);
        public const string RoleCannotBeCreateInDiscord = nameof(RoleCannotBeCreateInDiscord);

        public const string KeyAndOrIvMissingInConfiguration = nameof(KeyAndOrIvMissingInConfiguration);
    }
}
