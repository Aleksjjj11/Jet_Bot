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

        [Command("Say")]
        public async Task SayAsync()
        {
            await ReplyAsync("Hi", true);
        }

        [Command("To communicate")]
        public async Task ToCommunicateAsync()
        {
            DateTime date = DateTime.Now;

            int num = date.Millisecond % 3;
            string message = "";
            message += num.ToString();
            switch (num)
            {
                case 0:
                {
                    message += " How is it going?";
                    break;
                }
                case 1:
                {
                    message += " Long time no see!";
                    break;
                }
                case 2:
                {
                    message += " Until we meet again!";
                    break;
                }
            }
            await ReplyAsync(message, true);
        }

        
    }
}