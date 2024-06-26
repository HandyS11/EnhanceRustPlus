using Discord;
using Discord.Interactions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.Business.Parameters;

namespace EnhanceRustPlus.Commands
{
    public class SetCredentialsCommand(ICredentialsService credentialsService) : InteractionModuleBase
    {
        [SlashCommand("set-fcm-credentails", "Set fcm credentails")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        public async Task SetFcmCredentials([ComplexParameter] CredentialsParameter credentials)
        {
            _ = credentialsService.AddCredentails(credentials, Context.Guild.Id);

            var embed = new EmbedBuilder
            {
                Title = "Set FCM Credentails",
                Description = "Credentials added.",
                Color = Color.Green
            };
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}
