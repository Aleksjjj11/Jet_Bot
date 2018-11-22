using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Net;
using System.Net.Sockets;
using Discord.Rest;
using Discord.WebSocket;
using Jet_Bot.Core.Accounts;
using Newtonsoft.Json;

namespace Jet_Bot.Modules
{
    public class Admin : ModuleBase<SocketCommandContext>
    {
        [Command("Warn")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task WarnUserAsync(IGuildUser user)
        {
            if (CheckUserBeforeBan(user) && !IsUserAdmin(user))
            {
                var userAccount = UserAccounts.GetAccount((SocketUser) user);
                userAccount.NumberOfWarning++;
                UserAccounts.SaveAccounts();
                Console.WriteLine(UserAccounts.GetAccount((SocketUser) user).NumberOfWarning);

                if (userAccount.NumberOfWarning >= 3)
                {
                    var dmChannel = await user.GetOrCreateDMChannelAsync();
                    await dmChannel.SendMessageAsync("You have 3 warnings and banned for a day. Ban from Bot.");
                    await user.Guild.AddBanAsync(user, 1);
                    UserAccounts.GetAccount((SocketUser) user).NumberOfWarning = 0;
                }
                else
                {
                    var dmChannel = await user.GetOrCreateDMChannelAsync();
                    await dmChannel.SendMessageAsync("You given " + userAccount.NumberOfWarning.ToString() +
                                                     " warning.\n" +
                                                     "If there are 3 or more warnings, you will be issued a ban for a day. ");
                }
            }
            else
            {
                SendWarningAdmin();
            }
        }

        [Command("DelWarn")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task DelWarnUserAsync(IGuildUser user, uint count = 1)
        {
            var userAccount = UserAccounts.GetAccount((SocketUser) user);
            if (userAccount.NumberOfWarning < count)
            {
                count = userAccount.NumberOfWarning;
                userAccount.NumberOfWarning = 0;
            }
            else
            {
                userAccount.NumberOfWarning -= count;
            }
            UserAccounts.SaveAccounts();

            var dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(count.ToString() + " warnings were rempved from you.");
        }

        [Command("Mute")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        [RequireBotPermission(GuildPermission.MuteMembers)]
        public async Task MuteUserAsync(IGuildUser user)
        {
            if (CheckUserBeforeBan(user) && !IsUserAdmin(user))
            {
                var userAccount = UserAccounts.GetAccount((SocketUser) user);
                if (userAccount.IsMuted)
                {
                    var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                    await dmChannel.SendMessageAsync("User has already been issuedd a mute");
                }
                else
                {
                    userAccount.IsMuted = true;
                    UserAccounts.SaveAccounts();
                    var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                    await dmChannel.SendMessageAsync("Mute issued.");
                }

                var dmChannelFromUserMute = await user.GetOrCreateDMChannelAsync();
                await dmChannelFromUserMute.SendMessageAsync("You got a mute from the user " + Context.User.Username);
            }
            else
            {
                SendWarningAdmin();
            }
        }

        [Command("UnMute")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        [RequireBotPermission(GuildPermission.MuteMembers)]
        public async Task UnMuteUserAsync(IGuildUser user)
        {
            var userAccount = UserAccounts.GetAccount((SocketUser) user);
            if (userAccount.IsMuted)
            {
                userAccount.IsMuted = false;
                UserAccounts.SaveAccounts();
                var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("You removed the mute.");
            }
            else
            {
                var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("This user doesn't have a mute.");
            }
            var dmChannelFromUserMute = await user.GetOrCreateDMChannelAsync();
            await dmChannelFromUserMute.SendMessageAsync("You took a mute");
        }
        
        [Command("Kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickUserAsync(IGuildUser user, string reason = "No reason provided.")
        {
            if (CheckUserBeforeBan(user) && !IsUserAdmin(user))
            {
                await user.KickAsync(reason);
            }
            else
            {
                SendWarningAdmin();
            }
        }
        
        [Command("Ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(IGuildUser user, string reason = "No reason provided.", int count_day = 1)
        {
            if (CheckUserBeforeBan(user) && !IsUserAdmin(user))
            {
                //Ban can not be more than 7 days
                if (count_day > 7) count_day = 7;
                //Make information about ban
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Ban hammer")
                    .WithDescription(user.Username + " banned :hammer: for " + count_day.ToString() + " days.")
                    .WithColor(Color.Red)
                    .WithImageUrl("https://i0.wp.com/unitedworldgamers.org/wordpress1/wp-content/uploads/2017/12/dropping-the-ban-hammer-12-games-banned-across-the-world.jpg");
                //Send information about ban 
                await Context.Channel.SendMessageAsync("", false, builder);
                var dmChannel = await user.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("You banned for " + count_day.ToString() + " days.\nReason: " + reason);
                //Ban user
                await user.Guild.AddBanAsync(user, count_day, reason);
            }
            else
            {
                SendWarningAdmin();
            }
        }

        [Command("UnBan")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task UnBanUserAsync(string nameUser)
        {
            //UnBan user 
            var banList = Context.Guild.GetBansAsync().Result.ToArray();
            RestBan banUser = null;
            int i = 0;
            
            while (i < banList.Length)
            {
                if (banList[i].User.Username.ToUpper() == nameUser.ToUpper())
                {
                    banUser = banList[i];
                    break;
                } 
                i++;
            }
            
            if (banUser != null)
            {
                await Context.Guild.RemoveBanAsync(banUser.User);    
                var dmChannel = await banUser.User.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("You unbanned, thx for late. Good luck :kissing_heart: .");
            }
            else
            {
                await ReplyAsync("User not found.");
            }
        } 

        private bool CheckUserBeforeBan(IGuildUser user)
        {
            if (Context.User != (SocketUser) user)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void SendWarningAdmin()
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync("This command cannot be applied to this user.");
        }
 
        private bool IsUserAdmin(IGuildUser user)
        {
            return user.GuildPermissions.Administrator; 
        }
    }
}