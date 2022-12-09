using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NDB.Main
{
    public class MinLoader : ModuleBase<SocketCommandContext>
    {

        [Command("minload")]
        [Summary("OWNER: A minimal loading command, only supports loading commands (not services).")]
        [Remarks("minload <library>")]
        public async Task MinLoadCommand(String libraryToLoad)
        {
            await NDB_Main._commands.AddModulesAsync(Assembly.LoadFrom(libraryToLoad), NDB_Main._services);
            await ReplyAsync($"Loaded library: {libraryToLoad}...");
        }

        [Command("minunload")]
        [Summary("OWNER: A minimal unloading command, only supports unloading commands (not services).")]
        [Remarks("minunload <library>")]
        public async Task MinUnloadCommand(String libraryToUnload)
        {
            bool hasUnloaded = false;
            foreach (ModuleInfo module in NDB_Main._commands.Modules)
            {
                if (module.Name == libraryToUnload) { await NDB_Main._commands.RemoveModuleAsync(module); hasUnloaded = true; break; }
            }
            if (hasUnloaded)
            {
                await ReplyAsync($"Unloaded {libraryToUnload}.");
            } else
            {
                await ReplyAsync($"Failed to unload {libraryToUnload}.");
            }
        }
    }
}
