namespace TwitchUserList;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Program started");
        var dataAccess = new DataAccess();
        var config = dataAccess.GetConfig();
        Task.Delay(-1);
    }
}