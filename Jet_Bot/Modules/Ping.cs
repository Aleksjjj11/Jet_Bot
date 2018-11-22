using System;
using System.Collections.Immutable;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Google.Cloud.Translation.V2;

namespace Jet_Bot.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("Ping")]
        public async Task PingAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Ping.!.")
                .WithDescription("your ping very well")
                .WithColor(Color.Teal)
                .WithTimestamp(DateTimeOffset.Now);
            
            await ReplyAsync("", false, builder.Build());
        }
        
        [Command("Trigger")]
        public async Task TrigerAsync()
        { 
            await Context.Channel.SendMessageAsync(Context.IsPrivate.ToString());
            //var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            //await dmChannel.SendMessageAsync("Hi, how are you?");
        }
    }
}