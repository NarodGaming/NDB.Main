# A foundation for a Discord Bot (Discord.NET)

NDB provides a basic foundation for you to base a Discord.NET bot from.

It supports numerous additional libraries that can get your bot up & running much faster, including:

- NDB.Loader - a well-featured loader library so you can load in your other commands & dependencies.
- NDB.Library.Embeds - a simple embed library to beautify your messages quickly.
- NDB.Library.NScript - an experimental scripting language, allowing you to write commands in NScript.

## Bots

Ignim is a bot developed by myself that uses NDB and it's assorted libraries, along with custom properitary command modules to be a fully functional Discord bot.

Want to give it a try? [Add Ignim to your server](https://discord.com/api/oauth2/authorize?client_id=509490862498250781&permissions=1099565329414&scope=bot)

Try running `=help all` for all commands.

## Setup

NDB.Main itself is designed to be the foundation, and as such you should use either the included MinLoader or NDB.Loader to load in your additional modules & dependencies.

This means no modification is necessary to get NDB.Main up and running. See steps below:

1. Download the latest version of NDB.Main
2. Place it in an empty folder, and unzip your download to this folder.
3. Create a new file, called `config.json`
4. Open this new file, and insert your config as below:

```
{
    "token": "INSERT-YOUR-DISCORD-BOT-TOKEN-HERE",
    "presence": "INSERT-PRESENCE-MESSAGE-HERE",
    "stringprefix": "INSERT-STRING-PREFIX-HERE",
    "loaderlib": "OPTIONAL-LOADER-LIB-HERE"
}
```

A quick explanation of each:

- Token: This is your token provided by the Discord Developer Portal
- Presence: This appears underneath your bot in Discord. It will show as a status message.
- StringPrefix: This is how your bot should be triggered. Aka if you have a command caled "help" and a string prefix of "+", you would use "+help" to trigger your command.
- LoaderLib: If you're using NDB.Loader or a different loader, put the file name here.

Certain other libraries may also have additional configuration to add, such as NDB.Library.Embeds, which also expects "botname" and "embedcolour" to be set.

After this is all set-up, run NDB.Main.exe and enjoy! If you've added your bot to a server, you should see the bot come online with your given presence.

NDB.Main features no commands (apart from MinLoader, if you specified no LoaderLib), so get some loaded in! Don't know how to code? Add in NDB.Library.NScript and get scripting!

## What is MinLoader?

If you specify no loaderlib, MinLoader will instead serve as your loader for adding in additional commands. Unlike NDB.Loader, you are unable to truly unload libraries, and you cannot load in dependencies in a shared manner.

You can use `minload` to load in a library. `minunload` will remove it from being used, but does not unload it from memory or your bot.

You can find out how to write libraries with the Discord.NET documentation.