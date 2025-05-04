using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using Serilog;

namespace LuaSTGEditorSharp.Windows
{
    using Application = System.Windows.Application;
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        App mainApp = Application.Current as App;

        static readonly int[] resX = { 640, 960, 1280 };
        static readonly int[] resY = { 480, 720, 960 };
        readonly ObservableCollection<string> pluginPaths = [];

        public Array ThemeArray => new List<string>() { "LightTheme", "DarkTheme" }.ToArray();

        private static ILogger Logger = EditorLogging.ForContext("SettingsWindow");

        #region Settings

        private bool ignoreTHLibWarn;
        public bool IgnoreTHLibWarn
        {
            get => ignoreTHLibWarn;
            set
            {
                ignoreTHLibWarn = value;
                RaisePropertyChanged("IgnoreTHLibWarn");
            }
        }

        private bool useFolderPacking;
        public bool UseFolderPacking
        {
            get => useFolderPacking;
            set
            {
                useFolderPacking = value;
                RaisePropertyChanged("IgnoreTHLibWarn");
            }
        }

        private string zipExecutablePath;
        public string ZipExecutablePath
        {
            get => zipExecutablePath;
            set
            {
                zipExecutablePath = value;
                RaisePropertyChanged("ZipExecutablePath");
            }
        }

        private string luaSTGExecutablePath;
        public string LuaSTGExecutablePath
        {
            get => luaSTGExecutablePath;
            set
            {
                luaSTGExecutablePath = value;
                RaisePropertyChanged("LuaSTGExecutablePath");
            }
        }

        private string editorOutputName;
        public string EditorOutputName
        {
            get => editorOutputName;
            set
            {
                editorOutputName = value;
                RaisePropertyChanged("LuaSTGExecutablePath");
            }
        }

        private string tempPath;
        public string TempPath
        {
            get => tempPath;
            set
            {
                tempPath = value;
                RaisePropertyChanged("TempPath");
            }
        }

        private int debugResolutionX;
        public int DebugResolutionX
        {
            get => debugResolutionX;
            set
            {
                debugResolutionX = value;
                RaisePropertyChanged("DebugResolutionX");
            }
        }

        private int debugResolutionY;
        public int DebugResolutionY
        {
            get => debugResolutionY;
            set
            {
                debugResolutionY = value;
                RaisePropertyChanged("DebugResolutionY");
            }
        }

        public string CombinedResolution
        {
            get => DebugResolutionX + "x" + DebugResolutionY;
            set
            {
                string[] vs = value.Split('x');
                if (vs != null && vs.Count() > 1)
                {
                    if (int.TryParse(vs[0], out int x))
                    {
                        DebugResolutionX = x;
                    }
                    if (int.TryParse(vs[1], out int y))
                    {
                        DebugResolutionY = y;
                    }
                    RaisePropertyChanged("CombinedResolution");
                }
            }
        }

        public int IndexedReso
        {
            get
            {
                switch (DebugResolutionX)
                {
                    case 640:
                        return 0;
                    case 960:
                        return 1;
                    case 1280:
                        return 2;
                    default:
                        return -1;
                }
            }
            set
            {
                if (value != -1)
                {
                    DebugResolutionX = resX[value];
                    DebugResolutionY = resY[value];
                    RaisePropertyChanged("IndexedReso");
                }
            }
        }

        private bool debugWindowed;
        public bool DebugWindowed
        {
            get => debugWindowed;
            set
            {
                debugWindowed = value;
                RaisePropertyChanged("DebugWindowed");
            }
        }

        private bool debugCheat;
        public bool DebugCheat
        {
            get => debugCheat;
            set
            {
                debugCheat = value;
                RaisePropertyChanged("DebugCheat");
            }
        }
        private bool subLogWindow;
        public bool SubLogWindow
        {
            get => subLogWindow;
            set
            {
                subLogWindow = value;
                RaisePropertyChanged("SubLogWindow");
            }
        }

        private bool debugUpdateLib;
        public bool DebugUpdateLib
        {
            get => debugUpdateLib;
            set
            {
                debugUpdateLib = value;
                RaisePropertyChanged("DebugUpdateLib");
            }
        }

        private bool debugSaveProj;
        public bool DebugSaveProj
        {
            get => debugSaveProj;
            set
            {
                debugSaveProj = value;
                RaisePropertyChanged("DebugSaveProj");
            }
        }

        private bool packProj;
        public bool PackProj
        {
            get => packProj;
            set
            {
                packProj = value;
                RaisePropertyChanged("PackProj");
            }
        }

        private bool autoMoveToNew;
        public bool AutoMoveToNew
        {
            get => autoMoveToNew;
            set
            {
                autoMoveToNew = value;
                RaisePropertyChanged("AutoMoveToNew");
            }
        }

        private bool md5Check;
        public bool MD5Check
        {
            get => md5Check;
            set
            {
                md5Check = value;
                RaisePropertyChanged("MD5Check");
            }
        }

        private string authorName;
        public string AuthorName
        {
            get => authorName;
            set
            {
                authorName = value;
                RaisePropertyChanged("AuthorName");
            }
        }

        private bool batchPacking;
        public bool BatchPacking
        {
            get => batchPacking;
            set
            {
                batchPacking = value;
                RaisePropertyChanged("BatchPacking");
            }
        }

        private string pluginPath;
        public string PluginPath
        {
            get => pluginPath;
            set
            {
                pluginPath = value;
                if (pluginPath.Contains("lib\\") == true)
                {
                    pluginPath = value;
                } else
                {
                    pluginPath = "lib\\" + value;
                }
                
                RaisePropertyChanged("PluginPath");
            }
        }

        private bool spaceIndentation;
        public bool SpaceIndentation
        {
            get => spaceIndentation;
            set
            {
                spaceIndentation = value;
                RaisePropertyChanged("SpaceIndentation");
                RaisePropertyChanged("TabIndentation");
            }
        }

        public bool TabIndentation
        {
            get => !spaceIndentation;
            set
            {
                spaceIndentation = !value;
                RaisePropertyChanged("TabIndentation");
                RaisePropertyChanged("SpaceIndentation");
            }
        }

        private int indentationSpaceLength;
        public int IndentationSpaceLength
        {
            get => indentationSpaceLength;
            set
            {
                indentationSpaceLength = value;
                RaisePropertyChanged("IndentationSpaceLength");
            }
        }

        private bool dynamicDebugReporting;
        public bool DynamicDebugReporting
        {
            get => dynamicDebugReporting;
            set
            {
                dynamicDebugReporting = value;
                RaisePropertyChanged("DynamicDebugReporting");
            }
        }

        private bool useAutoSave;
        public bool UseAutoSave
        {
            get => useAutoSave;
            set
            {
                useAutoSave = value;
                RaisePropertyChanged("UseAutoSave");
            }
        }

        private int autoSaveTimer;
        public int AutoSaveTimer
        {
            get => autoSaveTimer;
            set
            {
                autoSaveTimer = value;
                RaisePropertyChanged("AutoSaveTimer");
            }
        }

        private string currentTheme;
        public string CurrentTheme
        {
            get => currentTheme;
            set
            {
                currentTheme = value;
                RaisePropertyChanged("CurrentTheme");
            }
        }

        private bool checkUpdateAtLaunch;
        public bool CheckUpdateAtLaunch
        {
            get => checkUpdateAtLaunch;
            set
            {
                checkUpdateAtLaunch = value;
                RaisePropertyChanged("CheckUpdateAtLaunch");
            }
        }

        private bool useDiscordRpc;
        public bool UseDiscordRpc
        {
            get => useDiscordRpc;
            set
            {
                useDiscordRpc = value;
                RaisePropertyChanged("UseDiscordRpc");
            }
        }

        private bool useRemoteTemplates;
        public bool UseRemoteTemplates
        {
            get => useRemoteTemplates;
            set
            {
                useRemoteTemplates = value;
                RaisePropertyChanged("UseRemoteTemplates");
            }
        }

        #endregion
        #region InSettings

        public bool IgnoreTHLibWarnSettings
        {
            get => mainApp.IgnoreTHLibWarn;
            set => mainApp.IgnoreTHLibWarn = value;
        }
        public bool UseFolderPackingSettings
        {
            get => mainApp.UseFolderPacking;
            set => mainApp.UseFolderPacking = value;
        }
        public string ZipExecutablePathSettings
        {
            get => mainApp.ZipExecutablePath;
            set => mainApp.ZipExecutablePath = value;
        }
        public string EditorOutputNameSettings
        {
            get => mainApp.EditorOutputName;
            set => mainApp.EditorOutputName = value;
        }

        public string LuaSTGExecutablePathSettings
        {
            get => mainApp.LuaSTGExecutablePath;
            set => mainApp.LuaSTGExecutablePath = value;
        }

        public string TempPathSettings
        {
            get => mainApp.TempPath;
            set => mainApp.TempPath = value;
        }

        public int DebugResolutionXSettings
        {
            get => mainApp.DebugResolutionX;
            set => mainApp.DebugResolutionX = value;
        }

        public int DebugResolutionYSettings
        {
            get => mainApp.DebugResolutionY;
            set => mainApp.DebugResolutionY = value;
        }

        public bool DebugWindowedSettings
        {
            get => mainApp.DebugWindowed;
            set => mainApp.DebugWindowed = value;
        }

        public bool DebugCheatSettings
        {
            get => mainApp.DebugCheat;
            set => mainApp.DebugCheat = value;
        }
        public bool SubLogWindowSettings
        {
            get => mainApp.SubLogWindow;
            set => mainApp.SubLogWindow = value;
        }

        public bool DebugUpdateLibSettings
        {
            get => mainApp.DebugUpdateLib;
            set => mainApp.DebugUpdateLib = value;
        }

        public bool DebugSaveProjSettings
        {
            get => mainApp.DebugSaveProj;
            set => mainApp.DebugSaveProj = value;
        }

        public bool PackProjSettings
        {
            get => mainApp.PackProj;
            set => mainApp.PackProj = value;
        }

        public bool AutoMoveToNewSettings
        {
            get => mainApp.AutoMoveToNew;
            set => mainApp.AutoMoveToNew = value;
        }

        public bool MD5CheckSettings
        {
            get => mainApp.SaveResMeta;
            set => mainApp.SaveResMeta = value;
        }

        public string AuthorNameSettings
        {
            get => mainApp.AuthorName;
            set => mainApp.AuthorName = value;
        }

        public bool BatchPackingSettings
        {
            get => mainApp.BatchPacking;
            set => mainApp.BatchPacking = value;
        }

        public string PluginPathSettings
        {
            get => mainApp.PluginPath;
            set => mainApp.PluginPath = value;
        }

        public bool DynamicDebugReportingSettings
        {
            get => mainApp.DynamicDebugReporting;
            set => mainApp.DynamicDebugReporting = value;
        }

        public bool SpaceIndentationSettings
        {
            get => mainApp.SpaceIndentation;
            set => mainApp.SpaceIndentation = value;
        }

        public int IndentationSpaceLengthSettings
        {
            get => mainApp.IndentationSpaceLength;
            set => mainApp.IndentationSpaceLength = value;
        }

        public string EditorthemeSettings
        {
            get => mainApp.Editortheme;
            set => mainApp.Editortheme = value;
        }

        public bool UseAutoSaveSettings
        {
            get => mainApp.UseAutoSave;
            set => mainApp.UseAutoSave = value;
        }

        public int AutoSaveTimerSettings
        {
            get => mainApp.AutoSaveTimer;
            set => mainApp.AutoSaveTimer = value;
        }

        public string CurrentThemeSettings
        {
            get => mainApp.CurrentTheme;
            set => mainApp.CurrentTheme = value;
        }

        public bool CheckUpdateAtLaunchSettings
        {
            get => mainApp.CheckUpdateAtLaunch;
            set => mainApp.CheckUpdateAtLaunch = value;
        }

        public bool UseDiscordRpcSettings
        {
            get => mainApp.UseDiscordRpc;
            set => mainApp.UseDiscordRpc = value;
        }

        public bool UseRemoteTemplatesSettings
        {
            get => mainApp.UseRemoteTemplates;
            set => mainApp.UseRemoteTemplates = value;
        }

        #endregion
        #region Integer box integration

        private void MaskNumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !TextIsNumeric(e.Text);
        }

        private void MaskNumericPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string input = (string)e.DataObject.GetData(typeof(string));
                if (!TextIsNumeric(input)) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool TextIsNumeric(string input)
        {
            return input.All(c => Char.IsDigit(c) || Char.IsControl(c));
        }

        #endregion

        public string TargetVersion
        {
            get => GetTargetVersion();
            set => SetTargetVersion();
        }

        private string GetTargetVersion()
        {
            if (!File.Exists(LuaSTGExecutablePath))
            {
                return "No LuaSTG exectuable set yet.";
            }
            FileVersionInfo LuaSTGExecutableInfos = FileVersionInfo.GetVersionInfo(LuaSTGExecutablePath);
            return $"{LuaSTGExecutableInfos.ProductName} v{LuaSTGExecutableInfos.ProductVersion}";
        }

        private void SetTargetVersion()
        {
            FileVersionInfo LuaSTGExecutableInfos = FileVersionInfo.GetVersionInfo(LuaSTGExecutablePath);

            string PluginNameInte;
            if (LuaSTGExecutableInfos.ProductName.Contains("Plus"))
                PluginNameInte = "lib\\LuaSTGPlusLib.dll";
            else if (LuaSTGExecutableInfos.ProductName.Contains("Sub"))
                PluginNameInte = "lib\\LuaSTGSubLib.dll";
            else if (LuaSTGExecutableInfos.ProductName.Contains("-x"))
                PluginNameInte = "lib\\LuaSTGXLib.Legacy.dll";
            else if (LuaSTGExecutableInfos.ProductName.Contains("Evo"))
                PluginNameInte = "lib\\LuaSTGEvoLib.dll";
            else
                PluginNameInte = "lib\\LuaSTGLib.Default.dll";

            if (PluginNameInte != PluginPath)
                System.Windows.MessageBox.Show("Warning: The editor must be restarted for these changes to work.");
            PluginPath = PluginNameInte;
        }

        private void WriteSettings()
        {
            UseFolderPackingSettings = UseFolderPacking;
            IgnoreTHLibWarnSettings = IgnoreTHLibWarn;
            AuthorNameSettings = AuthorName;
            AutoMoveToNewSettings = AutoMoveToNew;
            BatchPackingSettings = BatchPacking;
            DebugCheatSettings = DebugCheat;
            DebugResolutionXSettings = DebugResolutionX;
            DebugResolutionYSettings = DebugResolutionY;
            DebugSaveProjSettings = DebugSaveProj;
            DebugUpdateLibSettings = DebugUpdateLib;
            DebugWindowedSettings = DebugWindowed;
            LuaSTGExecutablePathSettings = LuaSTGExecutablePath;
            MD5CheckSettings = MD5Check;
            PackProjSettings = PackProj;
            PluginPathSettings = PluginPath;
            TempPathSettings = TempPath;
            ZipExecutablePathSettings = ZipExecutablePath;
            EditorOutputNameSettings = EditorOutputName;
            SpaceIndentationSettings = SpaceIndentation;
            IndentationSpaceLengthSettings = IndentationSpaceLength;
            SubLogWindowSettings = SubLogWindow;
            AutoSaveTimerSettings = AutoSaveTimer;
            UseAutoSaveSettings = UseAutoSave;
            CurrentThemeSettings = CurrentTheme;
            CheckUpdateAtLaunchSettings = CheckUpdateAtLaunch;
            UseDiscordRpcSettings = UseDiscordRpc;
            UseRemoteTemplatesSettings = UseRemoteTemplates;

            Logger.Information("Settings saved.");
        }

        private void ReadSettings()
        {
            UseFolderPacking = UseFolderPackingSettings;
            IgnoreTHLibWarn = IgnoreTHLibWarnSettings;
            AuthorName = AuthorNameSettings;
            AutoMoveToNew = AutoMoveToNewSettings;
            BatchPacking = BatchPackingSettings;
            DebugCheat = DebugCheatSettings;
            DebugResolutionX = DebugResolutionXSettings;
            DebugResolutionY = DebugResolutionYSettings;
            DebugSaveProj = DebugSaveProjSettings;
            DebugUpdateLib = DebugUpdateLibSettings;
            DebugWindowed = DebugWindowedSettings;
            LuaSTGExecutablePath = LuaSTGExecutablePathSettings;
            MD5Check = MD5CheckSettings;
            PackProj = PackProjSettings;
            PluginPath = PluginPathSettings;
            TempPath = TempPathSettings;
            ZipExecutablePath = ZipExecutablePathSettings;
            EditorOutputName = EditorOutputNameSettings;
            SpaceIndentation = SpaceIndentationSettings;
            IndentationSpaceLength = IndentationSpaceLengthSettings;
            SubLogWindow = SubLogWindowSettings;
            AutoSaveTimer = AutoSaveTimerSettings;
            UseAutoSave = UseAutoSaveSettings;
            CurrentTheme = CurrentThemeSettings;
            CheckUpdateAtLaunch = CheckUpdateAtLaunchSettings;
            UseDiscordRpc = UseDiscordRpcSettings;
            UseRemoteTemplates = UseRemoteTemplatesSettings;
        }

        public SettingsWindow()
        {
            ReadSettings();
            InitializeComponent();

            HashSet<string> names = [];
            var dependencyFiles =
                from string s
                in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
                where Path.GetExtension(s) == ".dependencies"
                select Path.GetFileNameWithoutExtension(s);
            foreach(string s in dependencyFiles)
            {
                if (!names.Contains(s)) names.Add(s);
            }

            //new List<string>(pathIgnorance).ForEach((s)=>System.Windows.MessageBox.Show(s));
            //System.Windows.MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);

            pluginPaths = [.. from string s
                in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\lib")
                where Path.GetExtension(s) == ".dll" && names.Contains(Path.GetFileNameWithoutExtension(s))
                select Path.GetFileName(s)
            ];
            //PluginList.ItemsSource = pluginPaths;
        }

        public SettingsWindow(int i) : this()
        {
            switch (i)
            {
                case 0:
                    GeneralTab.IsSelected = true;
                    break;
                case 1:
                    CompilerTab.IsSelected = true;
                    break;
                case 2:
                    DebugTab.IsSelected = true;
                    break;
                case 3:
                    EditorTab.IsSelected = true;
                    break;
                default:
                    GeneralTab.IsSelected = true;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void ButtonZipExecutablePath_Click(object sender, RoutedEventArgs e)
        {
            var chooseExc = new OpenFileDialog()
            {
                Filter = "Zip Executable|7z.exe"
            };
            if (chooseExc.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                ZipExecutablePath = chooseExc.FileName;
            }
        }

        private void ButtonLuaSTGExecutablePath_Click(object sender, RoutedEventArgs e)
        {
            var chooseExc = new OpenFileDialog()
            {
                Filter = "LuaSTG Executable|*.exe"
            };
            if (chooseExc.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                LuaSTGExecutablePath = chooseExc.FileName;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            TargetVersion = GetTargetVersion();
            WriteSettings();
            Properties.Settings.Default.Save();
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            TargetVersion = GetTargetVersion();
            WriteSettings();
            Properties.Settings.Default.Save();
        }

        private void ButtonRegisterExt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process p = new()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "ExtensionRegister.exe",
                        CreateNoWindow = true
                    }
                };
                p.Start();
                p.WaitForExit();
            }
            catch { }
        }

        private void ThemeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox box = (System.Windows.Controls.ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)box.SelectedItem;
        }

        private void FolderPackingCheck_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
