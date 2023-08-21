using Discord.Commands;
using System.Reflection;

namespace NDB.Main
{
    public class MinLoader : ModuleBase<SocketCommandContext> // minloader will be enabled automatically if no loader is configured in `config.json` as `loaderlib`.
    {

        [Command("minload")]
        [Summary("OWNER: A minimal loading command, only supports loading commands (not services).")]
        [Remarks("minload <library>")]
        public async Task MinLoadCommand(String libraryToLoad)
        {
            await NDB_Main._commands.AddModulesAsync(Assembly.LoadFrom(libraryToLoad), NDB_Main._services); // this loads in the assembly and adds it as a module. there is no way to unload the assembly properly after this, only detach the modules
            await ReplyAsync($"Loaded library: {libraryToLoad}..."); // print out message confirming library was loaded. if it failed, you won't know other than a crash
        }

        [Command("minunload")]
        [Summary("OWNER: A minimal unloading command, only supports unloading commands (not services).")]
        [Remarks("minunload <library>")]
        public async Task MinUnloadCommand(String libraryToUnload)
        {
            bool hasUnloaded = false; // keep track of if we have found the library to unload
            foreach (ModuleInfo module in NDB_Main._commands.Modules) // for each loaded module
            {
                if (module.Name == libraryToUnload) { await NDB_Main._commands.RemoveModuleAsync(module); hasUnloaded = true; break; } // if the name matches what we're looking for, remove the module
            }
            if (hasUnloaded) // if the loop found the module
            {
                await ReplyAsync($"Unloaded {libraryToUnload}."); // print success message
            } else
            {
                await ReplyAsync($"Failed to unload {libraryToUnload}."); // print failure message
            }
        }
    }
}
