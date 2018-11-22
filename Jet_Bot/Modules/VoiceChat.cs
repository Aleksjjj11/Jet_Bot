using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Discord;
using YoutubeExtractor;
using Discord.Commands;
using Discord.WebSocket;

namespace Jet_Bot.Modules
{
    public class VoiceChat : ModuleBase<SocketCommandContext>
    {
        private VideoDownloader downloader;
        private TaskCompletionSource<bool> DownloadFinishedEvent = new TaskCompletionSource<bool>();
        private VideoInfo Videoinfo;
        public string FilePath;
        public SocketVoiceChannel voiceChannel;
        
        
        [Command("JoinVoice")]
        public async Task JoinVoiceAsync(IVoiceChannel channel)
        { 
            voiceChannel = Context.Guild.GetVoiceChannel(channel.Id);
            await ReplyAsync(voiceChannel.Name + " ID: " + voiceChannel.Id.ToString());
            
            await voiceChannel.ConnectAsync();
            //SocketVoiceChannel vcChannel = (SocketVoiceChannel)channel;
            //vcChannel.Guild.
            //await ReplyAsync(name);
        }
        
        private static string GetMD5(string inputString) //Creates a MD5 hash from a input string
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(inputString);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();
            return encoded;
        }

        [Command("Dictionary")]
        public async Task Leave_vAsync()
        {
            await Context.Channel.SendFileAsync("dictionory.txt");
        } 
        
        private async Task DownloadVideo() //Downloads a Youtube Video
        {
            downloader = new VideoDownloader(Videoinfo, FilePath);
            downloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);
            try
            {
                downloader.Execute();
                DownloadFinishedEvent.SetResult(true);
            }
            catch (Exception DownloadException)
            {
                Console.WriteLine(DownloadException);
            }
            await DownloadFinishedEvent.Task;
        }
    }
}