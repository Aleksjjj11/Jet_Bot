using System.Collections.Generic;
using System.Linq;
using System;
using Discord.WebSocket;
using Jet_Bot.Modules;

namespace Jet_Bot.Core.Accounts
{
    public class UserAccounts
    {
        private static List<UserAccount> accounts;

        private static string accountFile = "Resources/accounts.json";
        
        static UserAccounts()
        {
            if (DataStorage_Accounts.SaveExists(accountFile))
            {
                accounts = DataStorage_Accounts.LoadUserAccounts(accountFile).ToList();
            }
            else
            {
                accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        public static void SaveAccounts()
        {
            DataStorage_Accounts.SaveUserAccounts(accounts, accountFile);
        }
        
        public static UserAccount GetAccount(SocketUser user)
        {
            //Console.WriteLine("ID in GetAccount : " + user.Id.ToString()); //Debug Completed
            return GetOrCreateAccount(user.Id, user.Username);
        }

        private static UserAccount CreateUserAccount(ulong id, string name)
        {
            var newAccount = new UserAccount();
            newAccount.ID = id; //Aaaaa одной строчки не хватало!!!!
            newAccount.UserName = name;
            
            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
        
        private static UserAccount GetOrCreateAccount(ulong id, string name)
        {
            //Console.WriteLine("ID in GetOrCreateAccount : " + id.ToString()); //Debug Completed
            var result = from a in accounts
                where a.ID == id
                where a.UserName == name
                select a;
            var account = result.FirstOrDefault();
            if (account == null) account = CreateUserAccount(id, name);
            return account;
        } 
    }
}