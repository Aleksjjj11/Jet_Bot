using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using System.Text;
using System.Threading;
using Discord;
using Discord.WebSocket;

namespace Jet_Bot.Modules
{
    public class Secret : ModuleBase<SocketCommandContext>
    {
        [Command("Secret")]
        public async Task SecretAsync([Remainder]string arg = "")
        {
            Console.WriteLine(UserIsBotOwner((SocketGuildUser)Context.User));
            if (!UserIsBotOwner((SocketGuildUser)Context.User)) return;
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync("Hi, how are you?");
            await ReplyAsync("Hi, soft maker!!");
        }

        [Command("UserList")]
        public async Task UserListAsync()
        {
            //Get list users sent in text channel
            if (!UserIsGod((SocketGuildUser) Context.User)) return;
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(DataStorage.GetDataStorage());
        }

        private bool UserIsBotOwner(SocketGuildUser user)
        {
            return UserIsRole(user, "BotOwner");
        }

        private bool UserIsGod(SocketGuildUser user)
        {
            return UserIsRole(user, "God");
        }

        private bool UserIsRole(SocketGuildUser user, string role)
        {
            string targetRoleName = role;
            var result = from r in user.Guild.Roles
                where r.Name == targetRoleName
                select r.Id;
            ulong roleID = result.FirstOrDefault();
            if (roleID == 0) return false; 
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
        }
         
    }
}