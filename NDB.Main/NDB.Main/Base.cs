using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class NDB_Main
{
    public static DiscordSocketClient? _client; // configure variables that will be set up in 'start' task
    public static CommandService? _commands;
    public static IConfiguration? _config;
    public static IServiceProvider? _services;

    public static List<Type>? _lateServices; // late servers, experimental

    public static String stringPrefix = "+"; // default stringprefix is '+', DONT change this here, change it in your 'config.json' as 'stringprefix'
    public static DateTime startTime = DateTime.Now; // keep track of the current time, feel free to fetch this in one of your commands to calculate uptime

    public static void LogMeOut() // command which can be run to logout, feel free to reference this in a shutdown command for a clean logout
    {
        _client.LogoutAsync();
    }

    private static void Main() // entrypoint
    {
        String[] strArg; // get any command line args passed and save them to this variable
        strArg = Environment.GetCommandLineArgs();

        Start().GetAwaiter().GetResult(); // run the start task and wait for it to complete (it never will)
    }

    private static async Task Start()
    {
        DiscordSocketConfig socketConfig = new() { GatewayIntents = GatewayIntents.All }; // watch for all gateway intents, your bot may need them all, might not
        _client= new DiscordSocketClient(socketConfig); // set up client and other needed commands
        _commands= new CommandService();
        _lateServices = new();
        _config = BuildConfig(); // fetch 'config.json'

        AddServices(); // add the services

        AttachHandlers(); // add the handlers

        if (_config["loaderlib"] == null) // if the config did not specify a loaderlib to use
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services); // then add this assembly as a module, which will add 'minloader'
        } else
        {
            await _commands.AddModulesAsync(Assembly.LoadFrom(_config["loaderlib"]), _services); // otherwise add the assembly specified as a module
        }
        

        if (_config["stringprefix"] != null) { stringPrefix = _config["stringprefix"]; } // if the config specified a stringprefix, use that instead of the default

        await _client.LoginAsync(TokenType.Bot, _config["token"]); // login to discord using the 'token' specified by 'config.json'
        await _client.StartAsync(); // start listening for commands

        if (_config["presence"] != null) { await _client.SetGameAsync(_config["presence"]); } // if the config specified a presence message, use that
        
        await Task.Delay(Timeout.Infinite); // leave this thread hanging forever (keeps the bot alive)
    }

    public static void AddServices()
    {
        ServiceCollection collection = new(); // create a service collection

        collection.AddSingleton(_client);
        collection.AddSingleton(_commands);

        collection.AddSingleton(_config);

        if(_lateServices.Count != 0) // if we need to add a late service
        {
            foreach (var item in _lateServices) // for each late service
            {
                collection.AddSingleton(item); // add the late service
            }
        }

        _services = collection.BuildServiceProvider();
    }

    private static void AttachHandlers() // handlers for logging & checking if commands have been received
    {
        _client.Log += Logger; // links the client logger up to our log function
        _commands.Log += Logger; // links the commands logger up to our log function

        _client.MessageReceived += CommandHandler; // links the messages the bot recieves from discord up to our command handler
    }

    private static IConfiguration BuildConfig()
    {
        return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json").Build(); // fetches the config.json in a neat way
    }

    private static async Task CommandHandler(SocketMessage message) // command handler
    {
        SocketUserMessage? userMessage = message as SocketUserMessage; // get the message received and convert it to SocketUserMessage

        if (userMessage == null || userMessage.Author.IsBot) { return; } // if the message is blank, or the message is from a different bot, ignore it

        SocketCommandContext context = new SocketCommandContext(_client, userMessage); // get the context of the message (aka the info on where it was ran, who ran it)

        int pos = 0; // var to keep track of where prefix ends and command starts

        if (userMessage.HasStringPrefix(stringPrefix, ref pos) || userMessage.HasMentionPrefix(_client.CurrentUser, ref pos)) // if the message mentions the bot, or starts with the listening stringprefix
        {
            IResult result = await _commands.ExecuteAsync(context, pos, _services); // run our bot on this command
        }
    }

    private static Task? Logger(LogMessage message) // logging function
    {
        Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}{Environment.NewLine}", true); // prints out to the console in a neat way
        return null;
    }
}