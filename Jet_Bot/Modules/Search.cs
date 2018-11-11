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

      private static IUser currentUser;
      //[Command("Search")]
      [Command("S")]
      [STAThread]
      public async Task SearchAsync(String message, int count_result)
      {
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
        searchListRequest.Q = message; // Replace with your search term.
        searchListRequest.MaxResults = count_result;

        // Call the search.list method to retrieve results matching the specified query term.
        var searchListResponse = await searchListRequest.ExecuteAsync();

        List<string> videos = new List<string>();
        List<string> channels = new List<string>();
        List<string> playlists = new List<string>();

        // Add each result to the appropriate list, and then display the lists of
        // matching videos, channels, and play lists.
      
        Program program = new Program(); //Create object programm for send messages
      
        foreach (var searchResult in searchListResponse.Items)
        {
            switch (searchResult.Id.Kind)
            {
              case "youtube#video":
              {
                videos.Add(String.Format("{0} {1}", searchResult.Snippet.Title, searchResult.Id.VideoId));
                break;
              }
              case "youtube#channel":
              {
                channels.Add(String.Format("{0} {1}", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                break;
              }
              case "youtube#playlist":
              {
                playlists.Add(String.Format("{0} {1}", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                break;
              } 
            }

          string crutch = searchResult.Id.Kind + "\t" + searchResult.Snippet.Title + "\t" + searchResult.Id.PlaylistId;
          Console.WriteLine(crutch);
          //SendMessages(searchResult.Id.VideoId);
          await ReplyAsync(crutch);
          await ReplyAsync("https://youtube.com/watch?v=" + searchResult.Id.VideoId);
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