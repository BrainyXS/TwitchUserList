using Newtonsoft.Json;

namespace TwitchUserList;

internal class DataAccess
{
    private const string PublisherName = "BrainySoftware";
    private const string SoftwareName = "TwitchUserList";

    public async Task<Config> GetConfigAsync()
    {
        var configDirectory = GetConfigDirectory();
        var filename = "config.json";

        var configPath = Path.Combine(configDirectory, filename);
        if (!File.Exists(configPath))
        {
            return await CreateNewConfigAsync(configDirectory, configPath);
        }

        return await ReadConfigAsync(configPath);
    }

    private async Task<Config> ReadConfigAsync(string configPath)
    {
        var text = await File.ReadAllTextAsync(configPath);
        var config = JsonConvert.DeserializeObject<Config>(text);
        return config;
    }

    private async Task<Config> CreateNewConfigAsync(string configDirectory, string configPath)
    {
        Console.WriteLine("Bitte geben Sie den Twitch Nutzername des Bot-Accounts ein");
        var twitchUsername = Console.ReadLine();
        Console.WriteLine("Bitte geben Sie ein entsprechendes Token ein");
        var twitchOauthToken = Console.ReadLine();
        Console.WriteLine("Bitte geben Sie ein Twitch Kanal ein, auf dem der Bot aktiv sein soll");
        var twitchChannel = Console.ReadLine();

        var config = new Config
        {
            TwitchUsername = twitchUsername!,
            TwitchChannelName = twitchChannel!,
            TwitchOauthToken = twitchOauthToken!
        };
        var json = JsonConvert.SerializeObject(config);

        Directory.CreateDirectory(configDirectory);
        await File.WriteAllTextAsync(configPath, json);
        return config;
    }

    private static string GetConfigDirectory()
    {
        var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var configPath = Path.Combine(appdataPath, PublisherName, SoftwareName);
        return configPath;
    }

    public async Task<bool> IsUserRegisteredAsync(string username)
    {
        var userfile = await GetUserFilePathAsync();
        var text = await File.ReadAllTextAsync(userfile);
        return text.Contains($"{username}\n");
    }

    private async Task<string> GetUserFilePathAsync()
    {
        var fileName = "users.txt";
        var directory = GetConfigDirectory();
        var filePath = Path.Combine(directory, fileName);
        if (!File.Exists(filePath))
        {
            await File.WriteAllTextAsync(filePath, string.Empty);
        }

        return filePath;
    }

    public async Task RegisterUserAsync(string username)
    {
        if (await IsUserRegisteredAsync(username))
        {
            return;
        }

        var file = await GetUserFilePathAsync();
        await File.AppendAllTextAsync(file, $"{username}\n");
    }
}