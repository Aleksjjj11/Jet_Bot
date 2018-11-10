using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Jet_Bot.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("Help")]
        public async Task HelpAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("All commands for bot")
                .WithDescription("_Translate_ *<message>* *<language>* - данная команда переводит введённое сообщение на выбранный язык.\n" +         
                "*T_en* *<message>* - данная команда переводит введённый текст с английского на русский.\n" +
                "*T_ru* *<message>* - данная команда переводит введённый текст с русского на английский.\n" +
                "_File_ - данная команда переводит архив из 5000 слов (_в разработке_)")
                .WithColor(Color.Teal)
                .WithImageUrl("https://png.pngtree.com/element_origin_min_pic/17/09/04/ede2dee23249b2cf239e56652bc42eb1.jpg")
                .WithTimestamp(DateTimeOffset.Now);
            
            await ReplyAsync("", false, builder.Build());
             
        }
    }
}