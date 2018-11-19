using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Jet_Bot.Modules
{
    public class ChatCorrecter : ModuleBase<SocketCommandContext>
    {        
        //Проверка сообщения на нецензурную лексику.
        public static bool CheckMessageAsync(SocketMessage message)
        { 
            var f = new StreamReader("Resources/Obsence2.txt", Encoding.Default);
            string dFile = ""; 
            while (dFile != null)
            {
                dFile = f.ReadLine();
                if (dFile != null)
                {
                    dFile = dFile.Replace("\n", "");
                    dFile = dFile.Replace(" ", "");

                    if (message.Content.ToUpper().IndexOf(dFile.ToUpper()) > -1) 
                    {
                        if (message.Content.ToUpper().IndexOf(dFile.ToUpper()) == 0)
                        {
                            return true;
                        }
                        else
                        {
                            if (message.Content[message.Content.ToUpper().IndexOf(dFile.ToUpper())-1] == ' '||
                                message.Content[message.Content.ToUpper().IndexOf(dFile.ToUpper())-1] == message.Content[message.Content.ToUpper().IndexOf(dFile.ToUpper())])
                                return true; 
                        }
                    }
                }
            }
            f.Close();

            return false;
        }
        //Add obscene words 
        [Command("AddObscene")]
        public async Task AddObsceneAsync(string word)
        {
            var f = new StreamReader("Resources/Obsence2.txt", Encoding.Default);
            string dFile = f.ReadToEnd();
            f.Close();
            dFile += "\n" + " " + word;
            System.IO.File.WriteAllText("Resources/Obsence2.txt", dFile);
        }

        
    }
}