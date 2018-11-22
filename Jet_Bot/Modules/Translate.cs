using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using Google.Apis.Util;
using Google.Cloud.Translation.V2;

namespace Jet_Bot.Modules
{
    public class Translate : ModuleBase<SocketCommandContext>
    {
        //This command created for testing work with API Google Translated
        [Command("Translate")]
        public async Task TranslateAsync(String language, [Remainder]String words)
        {
            TranslationClient client = TranslationClient.Create();
            var respone = client.TranslateText(words, language);
            await ReplyAsync(respone.TranslatedText, true);
            await ReplyAsync(words, false);
        }
        
        //This command created for simplify the command "Translate" 
        [Command("T_en")]
        public async Task T_EnAsync([Remainder]String word)
        {
            await TranslateAsync("en", word);
        }

        [Command("T_ru")]
        public async Task T_RuAsync([Remainder]String word)
        {
            await TranslateAsync("ru", word);
        }
        [Command("T_ja")]
        public async Task T_JaAsync([Remainder]String word)
        {
            await TranslateAsync("ja", word);
        }
        
        //This command created for testing work with files
        [Command("File")]
        public async Task FileAsync()
        { 
            try
            {
                StreamReader objReader = new StreamReader("dictionory.txt");
                string sLine="";
                ArrayList arrText = new ArrayList();
                while (sLine != null){
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                objReader.Close();

                foreach (string sOutput in arrText)
                {
                    //Translate message = new Translate();
                    //await ReplyAsync(sOutput, false);
                    System.Threading.Thread.Sleep(500);
                    await T_RuAsync(sOutput);
                    System.Threading.Thread.Sleep(3000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); 
            }
        }
    }
}