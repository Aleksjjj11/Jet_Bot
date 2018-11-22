using System.Collections.Generic;
using Discord.Commands;
using System.Text;
using Newtonsoft.Json;

namespace Jet_Bot.Modules
{
    public class DataStorage : ModuleBase<SocketCommandContext>
    {
        private static Dictionary<string, string> listUser = new Dictionary<string, string>();

        public static void AddToStorage(string key, string value)
        {
            listUser.Add(key, value);
            SaveDate();
        }

        public static string GetDataStorage()
        {
            return System.IO.File.ReadAllText("Users.json");
        }

        public static int GetCount()
        {
            return listUser.Count;
        }
        
        static DataStorage()
        {
            if(!ValideStorageFile("Users.json")) return;
            string json = System.IO.File.ReadAllText("Users.json");
            listUser = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static void SaveDate()
        {
            //Save new data
            string json = JsonConvert.SerializeObject(listUser, Formatting.Indented);
            System.IO.File.WriteAllText("Users.json", json);
        }

        private static bool ValideStorageFile(string file)
        {
            if (!System.IO.File.Exists(file))
            {
                System.IO.File.WriteAllText(file, "");
                SaveDate();
                return false;
            }

            return true;
        }
    }
}