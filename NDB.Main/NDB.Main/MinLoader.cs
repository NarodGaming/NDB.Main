using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDB.Main
{
    public class MinLoader : ModuleBase<SocketCommandContext>
    {

        [Command("minload")]
        [Summary("OWNER: A minimal loading command.")]
        [Remarks("minload (command)")]
        public async Task MinLoadCommand(String libraryToLoad)
        {
            await NDB_Main.minLoad(libraryToLoad);
            await ReplyAsync($"Attempted to load {libraryToLoad}...");
        }

        [Command("minunload")]
        [Summary("OWNER: A minimal unloading command.")]
        [Remarks("minunload (command)")]
        public async Task MinUnloadCommand()
        {
            await NDB_Main.minUnload();
            await ReplyAsync($"Attempted to unload MinLoader.");
        }
    }
}
