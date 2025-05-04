using System;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.Properties;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.Plugin.Default;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Windows;
using LuaSTGEditorSharp.Addons;
using System.Collections.Specialized;

namespace LuaSTGEditorSharp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application, IMessageThrowable, IAppSettings, IAppDebugSettings
    {
        public App()
        {
            PropertyChanged += new PropertyChangedEventHandler(CheckMessage);
        }

        public void InitDict()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            EditorLogging.Initialize();

            // Don't ask. I don't know. It just works. Start of the fuckery.
            ResourceDictionary r1 = new ResourceDictionary();
            r1.Source = new Uri($"pack://application:,,,/LuaSTGEditorThemes;component/{CurrentTheme}.xaml");
            Resources.MergedDictionaries.Add(r1);
            // End of the fuckery.

            TextWriter tw = Console.Out;
            try
            {
                string tempPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "LuaSTG Editor/"));
                if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
                using FileStream fs = new(Path.GetFullPath(Path.Combine(tempPath, "log.txt"))
                    , FileMode.OpenOrCreate, FileAccess.Write);
                using StreamWriter sw = new(fs);
                Console.SetOut(sw);
                base.OnStartup(e);

                bool addonsLoadedCorrectly = true;
                if (!AddonManager.TryLoadAddons())
                {
                    addonsLoadedCorrectly = false;
                }

                bool isPluginLoadedCorrectly = true;
                PluginHandler.DefaultPlugin = new DefaultPluginEntry();
                if (!PluginHandler.LoadPlugin(PluginPath))
                {
                    isPluginLoadedCorrectly = false;
                }
                LuaSTGEditorSharp.Windows.InputWindowSelector.Register(PluginHandler.Plugin.GetInputWindowSelectorRegister());
                LuaSTGEditorSharp.Windows.InputWindowSelector.AfterRegister();
                RaisePropertyChanged("m");

                Lua.SyntaxHighlightLoader.LoadLuaDef();

                var mainWindow = new MainWindow();
                //string arg = AppDomain.CurrentDomain.SetupInformation.ActivationArguments?.ActivationData?[0];
                string arg = "";
                if (e.Args.Length > 0)
                {
                    arg = e.Args[0];
                }
                if (!string.IsNullOrEmpty(arg))
                {
                    Uri fileUri = new Uri(arg);
                    string fp = Uri.UnescapeDataString(fileUri.AbsolutePath);
                    //MessageBox.Show(fp);
                    LoadDoc(fp);
                    //LoadDoc(arg);
                }

                MainWindow = mainWindow;
                MainWindow.Show();

                // If the plugin wasn't loaded correctly, display the MessageBox after creating the main window.
                // This way, it won't close instantly.
                if (!isPluginLoadedCorrectly)
                    MessageBox.Show("A plugin/target LuaSTG version has not been set approriately.\nPlease check your settings.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Current.Shutdown();
            }
        }

        public ObservableCollection<MessageBase> Messages { get; } = new ObservableCollection<MessageBase>();

        public IInputWindowSelectorRegister InputWindowSelector { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            if (!File.Exists(LuaSTGExecutablePath))
            {
                a.Add(new EXEPathNotSetMessage(LuaSTGExecutablePath, "LuaSTG Path", 0, this));
            }
            if (BatchPacking && (!File.Exists(ZipExecutablePath) || Path.GetFileName(ZipExecutablePath) != "7z.exe"))
            {
                a.Add(new EXEPathNotSetMessage(ZipExecutablePath, "7z Path", 0, this));
            }
            return a;
        }

        public void CheckMessage(object sender, PropertyChangedEventArgs e)
        {
            var a = GetMessage();
            Messages.Clear();
            foreach (MessageBase mb in a)
            {
                Messages.Add(mb);
            }
            MessageContainer.UpdateMessage(this);
        }

        public void LoadDoc(string arg)
        {
            (MainWindow as MainWindow).OpenDocByFile(arg);
        }
        public bool IgnoreTHLibWarn
        {
            get => Settings.Default.IgnoreTHLibWarn;
            set
            {
                Settings.Default["IgnoreTHLibWarn"] = value;
            }
        }
        public bool UseFolderPacking
        {
            get => Settings.Default.UseFolderPacking;
            set
            {
                Settings.Default["UseFolderPacking"] = value;
            }
        }

        public string ZipExecutablePath
        {
            get => Settings.Default.ZipExecutablePath;
            set
            {
                Settings.Default["ZipExecutablePath"] = value;
                RaisePropertyChanged("ZipExecutablePath");
            }
        }

        public string LuaSTGExecutablePath
        {
            get => Settings.Default.LuaSTGExecuteablePath;
            set
            {
                Settings.Default["LuaSTGExecuteablePath"] = value;
                RaisePropertyChanged("LuaSTGExecuteablePath");
            }
        }
        public string EditorOutputName
        {
            get => Settings.Default.EditorOutputName;
            set
            {
                Settings.Default["EditorOutputName"] = value;
                //RaisePropertyChanged("EditorOutputName");
            }
        }

        public int DebugResolutionX
        {
            get => Settings.Default.DebugResolutionX;
            set
            {
                Settings.Default["DebugResolutionX"] = value;
            }
        }

        public int DebugResolutionY
        {
            get => Settings.Default.DebugResolutionY;
            set
            {
                Settings.Default["DebugResolutionY"] = value;
            }
        }

        public bool DebugWindowed
        {
            get => Settings.Default.DebugWindowed;
            set
            {
                Settings.Default["DebugWindowed"] = value;
            }
        }

        public bool DebugCheat
        {
            get => Settings.Default.DebugCheat;
            set
            {
                Settings.Default["DebugCheat"] = value;
            }
        }

        public bool SubLogWindow
        {
            get => Settings.Default.SubLogWindow;
            set
            {
                Settings.Default["SubLogWindow"] = value;
            }
        }

        public bool DebugUpdateLib
        {
            //get => Settings.Default.DebugUpdateLib;
            get => false;
            set
            {
                Settings.Default["DebugUpdateLib"] = value;
            }
        }

        public bool DebugSaveProj
        {
            get => Settings.Default.DebugSaveProj;
            set
            {
                Settings.Default["DebugSaveProj"] = value;
            }
        }

        public bool PackProj
        {
            get => Settings.Default.PackProj;
            set
            {
                Settings.Default["PackProj"] = value;
            }
        }

        public bool SaveResMeta
        {
            get => Settings.Default.SaveResMeta;
            set
            {
                Settings.Default["SaveResMeta"] = value;
            }
        }

        public bool AutoMoveToNew
        {
            get => Settings.Default.AutoMoveToNew;
            set
            {
                Settings.Default["AutoMoveToNew"] = value;
            }
        }

        public string AuthorName
        {
            get => Settings.Default.AuthorName;
            set
            {
                Settings.Default["AuthorName"] = value;
            }
        }

        public string PluginPath
        {
            get => Settings.Default.LuaSTGNodeLibPath;
            set
            {
                Settings.Default["LuaSTGNodeLibPath"] = value;
            }
        }

        public string TempPath
        {
            get => Settings.Default.TempPath;
            set
            {
                Settings.Default["TempPath"] = value;
            }
        }

        public bool BatchPacking
        {
            get => !Settings.Default.UseInternalZipCompressor;
            set
            {
                Settings.Default["UseInternalZipCompressor"] = !value;
            }
        }

        public string SLDir
        {
            get => Settings.Default.SLDir;
            set
            {
                Settings.Default["SLDir"] = value;
            }
        }

        public bool DynamicDebugReporting
        {
            get => Settings.Default.DynamicDebugReporting;
            set
            {
                Settings.Default["DynamicDebugReporting"] = value;
            }
        }

        public bool SpaceIndentation
        {
            get => Settings.Default.SpaceIndentation;
            set
            {
                if (value)
                {
                    Lua.IndentationGenerator.Current = new Lua.SpaceIndentation() { NumOfSpaces = IndentationSpaceLength };
                }
                else
                {
                    Lua.IndentationGenerator.Current = new Lua.TabIndentation();
                }
                Settings.Default["SpaceIndentation"] = value;
            }
        }

        public int IndentationSpaceLength
        {
            get => Settings.Default.IndentationSpaceLength;
            set
            {
                if (Lua.IndentationGenerator.Current is Lua.SpaceIndentation)
                    (Lua.IndentationGenerator.Current as Lua.SpaceIndentation).NumOfSpaces = value;
                Settings.Default["IndentationSpaceLength"] = value;
            }
        }

        public string Editortheme
        {
            get => Settings.Default.Editortheme;
            set
            {
                Settings.Default["Editortheme"] = value;
            }
        }

        public bool IsEXEPathSet
        {
            get => !(BatchPacking && string.IsNullOrEmpty(ZipExecutablePath)) && !string.IsNullOrEmpty(LuaSTGExecutablePath);
        }

        public bool UseAutoSave
        {
            get => Settings.Default.UseAutoSave;
            set => Settings.Default.UseAutoSave = value;
        }

        public int AutoSaveTimer
        {
            get => Settings.Default.AutoSaveTimer;
            set => Settings.Default.AutoSaveTimer = value;
        }

        public string CurrentTheme
        {
            get => Settings.Default.CurrentTheme;
            set => Settings.Default.CurrentTheme = value;
        }

        public bool CheckUpdateAtLaunch
        {
            get => Settings.Default.CheckUpdateAtLaunch;
            set => Settings.Default.CheckUpdateAtLaunch = value;
        }

        public bool UseDiscordRpc
        {
            get => Settings.Default.UseDiscordRpc;
            set => Settings.Default.UseDiscordRpc = value;
        }

        public bool UseRemoteTemplates
        {
            get => Settings.Default.UseRemoteTemplates;
            set => Settings.Default.UseRemoteTemplates = value;
        }

        public StringCollection RecentlyOpened
        {
            get => Settings.Default.RecentlyOpened;
            set => Settings.Default.RecentlyOpened = value;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
            Current.Shutdown();
        }
    }
}
