using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Execution;

namespace LuaSTGEditorSharp
{
    public class LSTGSubExecution : LSTGExecution
    {
        public override void BeforeRun(ExecutionConfig config)
        {
            IAppDebugSettings currentApp = System.Windows.Application.Current as IAppDebugSettings;
            Parameter = "\""
                + $"Debug = true; "
                + $"Settings.Windowed = {currentApp.DebugWindowed.ToString().ToLower()}; "
                + $"Settings.ResX = {currentApp.DebugResolutionX}; "
                + $"Settings.ResY = {currentApp.DebugResolutionY}; "
                + $"Cheat = {currentApp.DebugCheat.ToString().ToLower()}; "
                + $"Updatelib = {currentApp.DebugUpdateLib.ToString().ToLower()}; "
                + $"Settings.Game = \'{config.ModName}\';\""

                + (currentApp.SubLogWindow ? "--log-window" : "");
            UseShellExecute = false;
            CreateNoWindow = true;
            RedirectStandardError = !currentApp.SubLogWindow;
            RedirectStandardOutput = !currentApp.SubLogWindow;
        }

        protected override string LogFileName => "engine.log";
    }
}
