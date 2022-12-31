using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TwitchUserList;

internal class TwitchBot
{
    private readonly Config _config;
    private readonly TwitchClient _client;
    private readonly DataAccess _dataAccess;

    public TwitchBot(Config config)
    {
        _config = config;
        _client = new TwitchClient();
        _dataAccess = new DataAccess();
    }

    public void Startup()
    {
        _client.Initialize(new ConnectionCredentials(_config.TwitchUsername, _config.TwitchOauthToken), _config.TwitchChannelName);
        _client.Connect();
        _client.OnMessageReceived += MessageRecieved;
    }

    private async void MessageRecieved(object sender, OnMessageReceivedArgs e)
    {
        if (e.ChatMessage.Message == "!join")
        {
            var username = e.ChatMessage.Username;
            if (await _dataAccess.IsUserRegisteredAsync(username))
            {
                Console.WriteLine($"Dont add {username} to List, because he is already in it");
                _client.SendMessage(_config.TwitchChannelName, $"@{username}, du bist bereits in der Liste");
            }
            else
            {
                Console.WriteLine($"Adding {username} to List");
                await _dataAccess.RegisterUserAsync(username);
                _client.SendMessage(_config.TwitchChannelName, $"@{username} in die Liste aufgenommen");
            }
        }
    }
}