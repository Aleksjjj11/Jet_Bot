using System;
using System.Threading.Tasks;
using Discord.Commands;
using Google.Cloud.Translation.V2;

namespace Jet_Bot.Modules
{
    public class Translate : ModuleBase<SocketCommandContext>
    {
        [Command("Translate")]
        public async Task TranslateAsync(String word, String language)
        {
            string words = "";
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != '-')
                    words += word[i];
                else words += ' ';
            }
            TranslationClient client = TranslationClient.Create();
            var respone = client.TranslateText(words, language);
            await ReplyAsync(respone.TranslatedText, true);
            await ReplyAsync(words, false);
        }

        [Command("T_en")]
        public async Task T_EnAsync(String word)
        {
            TranslateAsync(word, "ru");
        }

        [Command("T_ru")]
        public async Task T_RuAsync(String word)
        {
            TranslateAsync(word, "en");
        }
    }
}