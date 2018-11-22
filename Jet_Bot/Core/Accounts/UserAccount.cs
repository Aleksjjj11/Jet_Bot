namespace Jet_Bot.Core.Accounts
{
    public class UserAccount
    {
        public string UserName;
        
        public ulong ID;
        
        public bool IsMuted { get; set; }
        
        public uint NumberOfWarning { get; set; }
    }
}