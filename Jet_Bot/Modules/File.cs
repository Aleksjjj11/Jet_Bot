using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Jet_Bot.Modules
{
    public class File : ModuleBase<SocketCommandContext>
    {
        [Command("File")]
        public async Task FileAsync()
        {
            string text = "";
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
                    Translate message = new Translate();
                    await ReplyAsync(sOutput, true);
                    System.Threading.Thread.Sleep(500);
                    await message.T_EnAsync(sOutput);
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