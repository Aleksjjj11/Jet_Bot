using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.YouTube.v3;

namespace Jet_Bot.Modules
{
    public class Search : ModuleBase<SocketCommandContext>
    { 
      //[Command("Search")]
      [Command("S")]
      [STAThread]
      public async Task SearchAsync(int count_result = 1, [Remainder] String messages = "Music")
      {
        //await ReplyAsync(messages);
        
        if (count_result > 50)
        {
          await ReplyAsync("Кол-во возможных результатов слишком большое, будет выведенно только 50.");
          count_result = 50;
        }
        Search search = new Search();
        try
        {
          //search.Run(message);
          BaseClientService.Initializer baseClientService = new BaseClientService.Initializer()
        {
          ApiKey = "AIzaSyCZzg069XlgJ9_yIDwYkT1GTpi8aHRrh2U",
          ApplicationName = "Jet-Bot" 
        };
        YouTubeService youtubeService = new YouTubeService(baseClientService);

        var searchListRequest = youtubeService.Search.List("snippet");
        searchListRequest.Q = messages; // Replace with your search term.
        searchListRequest.MaxResults = count_result;

        // Call the search.list method to retrieve results matching the specified query term.
        var searchListResponse = await searchListRequest.ExecuteAsync();

        List<string> videos = new List<string>();
        List<string> channels = new List<string>();
        List<string> playlists = new List<string>();

        // Add each result to the appropriate list, and then display the lists of
        // matching videos, channels, and play lists. 
      
        foreach (var searchResult in searchListResponse.Items)
        {
          string crutch = searchResult.Id.Kind + "\t" + searchResult.Snippet.Title + "\t" + searchResult.Id.PlaylistId;
          await ReplyAsync(crutch);  
          switch (searchResult.Id.Kind)
            {
              case "youtube#video":
              {
                await ReplyAsync("https://youtube.com/watch?v=" + searchResult.Id.VideoId);
                videos.Add(String.Format("{0} {1}", searchResult.Snippet.Title, searchResult.Id.VideoId));
                break;
              }
              case "youtube#channel":
              {
                await ReplyAsync("https://youtube.com/channel/" + searchResult.Id.ChannelId);
                channels.Add(String.Format("{0} {1}", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                break;
              }
              case "youtube#playlist":
              {
                await ReplyAsync("https://youtube.com/playlist?list=" + searchResult.Id.PlaylistId);
                playlists.Add(String.Format("{0} {1}", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                break;
              } 
            }
          Console.WriteLine(crutch); 
        }
        }
        catch (AggregateException ex)
        {
          foreach (var e in ex.InnerExceptions)
          {
            Console.WriteLine("Error: " + e.Message);
          }
        }

        //await ReplyAsync("Completed", false);
    }
  }
}