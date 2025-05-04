using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace LuaSTGEditorSharp;

public static class EditorLogging
{
    public static ILogger CoreLogger;

    public static void Initialize()
    {
        string pathToLog = Path.Combine(Directory.GetCurrentDirectory(), "editor.log");
        if (File.Exists(pathToLog))
            File.Delete(pathToLog);

        const string template = "{Timestamp:dd-MM-yyyy HH:mm:ss} [{Level:u3}] [{Tag}] {Message:lj}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#endif
            .WriteTo.File(pathToLog,
                rollingInterval: RollingInterval.Infinite,
                outputTemplate: template,
                rollOnFileSizeLimit: true)
            .CreateLogger();

        CoreLogger = Log.ForContext("Tag", "Core");

        CoreLogger.Debug("Launching editor in DEBUG mode.");
    }

    public static ILogger ForContext(string name) => Log.ForContext("Tag", name);
}
