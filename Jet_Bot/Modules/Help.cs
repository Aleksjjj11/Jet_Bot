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
                .WithDescription("```css\n[Translate] <language> <message>\n{.: 'данная команда переводит введённое сообщение на выбранный язык.'}\n" +         
                "[T_en] <message>\n{.: 'данная команда переводит введённый текст с любого языка на русский.'}\n" +
                "[T_ru] <message>\n{.: 'данная команда переводит введённый текст с любого языка на английский.'}\n" +
                "[File]\n{.: 'данная команда переводит архив из 5000 слов (в разработке)'}\n"+
                "[S] <count_result> <request>\n{.: 'данная команда находит видео, каналы и плейлисты по заданному запросу(макс. знач. 50)'}\n```")
                .WithColor(7949007U)
                .WithImageUrl("https://images.ctfassets.net/awpxl2koull4/1pB4INeVpe2EOsoyOIak68/2ac6be9ffbba97e18e7da8b322568849/2652bc95-59e2-4283-b2b8-c832d656eca4.jpg?w=1200")
                .WithTimestamp(DateTimeOffset.Now);
            
            await ReplyAsync("", false, builder.Build());
            //DataStorage.AddToStorage(Context.User.Username, Context.User.Id.ToString());
             
        }

        [Command("ComList")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CommandListAdmin()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Command for admin")
                .WithDescription("```css\n" +
                                 "[ban] <user_name> <reason> <count_day> {.: Выдать пользователю бан на n дней}\n" +
                                 "[kick] <user_name> {.: выгнать пользователя с сервера}\n" +
                                 "[warn] <user_name> {.: выдать предупреждение пользователю}\n" +
                                 "[delWarn] <user_name> <count> {.: снять предупреждение с пользователя}\n" +
                                 "[mute] <user_name> {.: выдать мут пользователю.}\n" +
                                 "[unmute] <user_name> {.:снять мут с пользователя}\n" +
                                 "[unban] <user_name> {.:снять бан с пользователя}\n" +
                                 "```")
                .WithColor(new Color(152, 56, 209));
            await ReplyAsync("", false, builder.Build());
        }
    }
}