using System.IO;
using System.Numerics;
using System.Reflection;
using System.Threading;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class NDB_Main
{
    public static DiscordSocketClient? _client;
    public static CommandService? _commands;
    private static IConfiguration? _config;
    private static IServiceProvider? _services;

    private static String stringPrefix = "+";

    public static Task minLoad(String libraryToLoad) // loads minloader (MinLoader.cs)
    {
        return _commands.AddModulesAsync(Assembly.LoadFrom(libraryToLoad), _services);
    }

    public static Task minUnload() // unloads minloader (MinLoader.cs)
    {
        return _commands.RemoveModuleAsync(_commands.Modules.ElementAt(0));
    }

    public static void LogMeOut()
    {
        _client.LogoutAsync();
    }

    private static void Main()
    {
        String[] strArg;
        strArg = Environment.GetCommandLineArgs();

        Start().GetAwaiter().GetResult();
    }

    private static async Task Start()
    {
        DiscordSocketConfig socketConfig = new() { GatewayIntents = GatewayIntents.All };
        _client= new DiscordSocketClient(socketConfig);
        _commands= new CommandService();
        _config = BuildConfig();

        AddServices();

        AttachHandlers();

        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        if (_config["stringprefix"] != null) { stringPrefix = _config["stringprefix"]; }

        await _client.LoginAsync(TokenType.Bot, _config["token"]);
        await _client.StartAsync();

        await _client.SetGameAsync(_config["presence"]);

        await Task.Delay(Timeout.Infinite);
    }

    private static void AddServices()
    {
        ServiceCollection collection = new ServiceCollection();

        collection.AddSingleton(_client);
        collection.AddSingleton(_commands);

        collection.AddSingleton(_config);

        _services = collection.BuildServiceProvider();
    }

    private static void AttachHandlers()
    {
        _client.Log += Logger;
        _commands.Log += Logger;

        _client.MessageReceived += CommandHandler;
    }

    private static IConfiguration BuildConfig()
    {
        return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json").Build();
    }

    private static async Task CommandHandler(SocketMessage message)
    {
        SocketUserMessage? userMessage = message as SocketUserMessage;
        SocketCommandContext context = new SocketCommandContext(_client, userMessage);

        if (userMessage == null || userMessage.Author.IsBot) { return; }

        int pos = 0;

        if (userMessage.HasStringPrefix("+", ref pos) || userMessage.HasMentionPrefix(_client.CurrentUser, ref pos))
        {
            IResult result = await _commands.ExecuteAsync(context, pos, _services);
        }
    }

    private static Task? Logger(LogMessage message)
    {
        Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}{Environment.NewLine}", true);
        return null;
    }
}