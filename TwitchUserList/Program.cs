using System;
using System.Threading.Tasks;

namespace TwitchUserList;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Program started");
        var dataAccess = new DataAccess();
        var config = await dataAccess.GetConfigAsync();
        var bot = new TwitchBot(config);
        bot.Startup();
        await Task.Delay(-1);
    }
}