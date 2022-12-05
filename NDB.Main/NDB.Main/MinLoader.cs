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
        [Summary("OWNER: A minimal loading command.")]
        [Remarks("minload <library>")]
        public async Task MinLoadCommand(String libraryToLoad)
        {
            await NDB_Main._commands.AddModulesAsync(Assembly.LoadFrom(libraryToLoad), NDB_Main._services);
            await ReplyAsync($"Loaded library: {libraryToLoad}...");
        }

        [Command("minunload")]
        [Summary("OWNER: A minimal unloading command.")]
        [Remarks("minunload")]
        public async Task MinUnloadCommand()
        {
            await NDB_Main._commands.RemoveModuleAsync(NDB_Main._commands.Modules.ElementAt(0));
            await ReplyAsync($"Unloaded MinLoader, if you haven't got a different loader activated, you won't be able to load / unload new modules anymore.");
        }
    }
}
