using System;
using System.Reflection;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace LuaSTGEditorSharp.Plugin
{
    public static class PluginHandler
    {
        private static ILogger Logger = EditorLogging.ForContext("PluginHandler");

        public static AbstractPluginEntry DefaultPlugin { get; set; }
        public static AbstractPluginEntry Plugin { get; private set; } = null;

        public static bool LoadPlugin(string PluginPath)
        {
            bool isSuccess;
            Assembly pluginAssembly = null;
            try
            {
                string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginPath));
                pluginAssembly = Assembly.LoadFile(path);
                Plugin = (AbstractPluginEntry)pluginAssembly.CreateInstance("LuaSTGEditorSharp.PluginEntry");
                
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Plugin initialization failed. Reason:\n{ex}");
            }
            if (Plugin == null)
            {
                Plugin = DefaultPlugin;
                Plugin.NodeTypeCache.Initialize(Assembly.GetExecutingAssembly());
                isSuccess = false;
                Logger.Information("Plugin initialization failed.");
            }
            else
            {
                Plugin.NodeTypeCache.Initialize(AppDomain.CurrentDomain.GetAssemblies());
                isSuccess = true;
                Logger.Information("Plugin initialization successful.");
            }
            return isSuccess;
        }
    }
}
