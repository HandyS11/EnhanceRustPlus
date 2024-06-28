using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnhanceRustPlus.EfCore.Entities
{
    [PrimaryKey(nameof(GuildId))]
    public class GuildConfig
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong GuildId { get; set; }
        public Guild Guild { get; set; } = null!;

        public ulong RolesId { get; set; }
        public ulong MainCategoryId { get; set; }

        public ulong ServersChannelId { get; set; }
        public ulong UsersChannelId { get; set; }
        public ulong SettingsChannelId { get; set; }
        public ulong CommandChannelId { get; set; }
    }
}
