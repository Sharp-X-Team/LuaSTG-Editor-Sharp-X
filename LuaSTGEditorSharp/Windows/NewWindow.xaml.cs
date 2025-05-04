using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Serilog;

namespace LuaSTGEditorSharp.Windows
{
    public struct RemoteTemplate {
        public string name { get; set; }
        public string download_url { get; set; }
    }

    /// <summary>
    /// NewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewWindow : Window, INotifyPropertyChanged
    {
        private string fileName = "Untitled";
        private string author = (Application.Current as App).AuthorName;
        private bool allowpr = true;
        private bool allowscpr = true;
        private int modversion = 4096;
        private static ILogger Logger = EditorLogging.ForContext("NewWindow");

        public string FileName
        {
            get => fileName;
            set
            {
                fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        public string Author
        {
            get => author;
            set
            {
                author = value;
                RaisePropertyChanged("Author");
            }
        }

        public bool AllowPR
        {
            get => allowpr;
            set
            {
                allowpr = value;
                RaisePropertyChanged("AllowPR");
            }
        }

        public bool AllowSCPR
        {
            get => allowscpr;
            set
            {
                allowscpr = value;
                RaisePropertyChanged("AllowSCPR");
            }
        }

        public int ModVersion
        {
            get => modversion;
            set
            {
                modversion = value;
                RaisePropertyChanged("ModVersion");
            }
        }

        public string SelectedPath { get; set; }

        class DefS
        {
            public string Text { get; set; }
            public string FullPath { get; set; }
            public string Icon { get; set; }
        }

        List<DefS> templates;

        HttpClient client;
        private Dictionary<string, string> remoteTemplateDescCache = [];

        public NewWindow()
        {
            string s = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\"));
            DirectoryInfo dir = new(s);
            List<FileInfo> fis = [.. dir.GetFiles("*.lstges"), .. dir.GetFiles("*.lstgproj")];
            templates = [.. from FileInfo fi
                in fis
                select new DefS {
                    Text = Path.GetFileNameWithoutExtension(fi.Name),
                    FullPath = fi.FullName,
                    Icon = "..\\images\\Icon.png"
                }
            ];

            try
            {
                if ((App.Current as App).UseRemoteTemplates)
                {
                    client = new();
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Sharp-X Editor");

                    Logger.Information("Gathering remote template files...");
                    string remoteJson = client.GetStringAsync("https://api.github.com/repos/Sharp-X-Team/Sharp-X-templates/contents/templates").Result;
                    var remoteTemplates = JsonConvert.DeserializeObject<List<RemoteTemplate>>(remoteJson);
                    Logger.Information($"Found {remoteTemplates.Count} remote templates");

                    templates.AddRange([.. from RemoteTemplate rt
                        in remoteTemplates
                        // Get all files that ends with ".lstges" and doesn't already exist in the local templates.
                        where (Path.GetExtension(rt.name) == ".lstges" || Path.GetExtension(rt.name) == "*.lstgproj")
                            && !templates.Any(x => x.Text == Path.GetFileNameWithoutExtension(rt.name))
                        select new DefS {
                            Text = Path.GetFileNameWithoutExtension(rt.name),
                            FullPath = rt.download_url,
                            Icon = "..\\images\\IconRemoteTemplate.png"
                        }
                    ]);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to gather remote templates. Reason:\n{ex}");
            }

            InitializeComponent();
            ListTemplates.ItemsSource = templates;
            try
            {
                ListTemplates.SelectedIndex = 0;
            }
            catch { }
            TextName.Focus();
            TextName.SelectAll();
        }

        private async void ListTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DefS sel = ListTemplates.SelectedItem as DefS;
            try
            {
                if (sel.FullPath.StartsWith("https://"))
                {
                    // Cache for avoiding to download the file each time.
                    if (!remoteTemplateDescCache.TryGetValue(sel.Text, out string contents))
                    {
                        string ext = Path.ChangeExtension(sel.FullPath, "txt");
                        // Don't replace the first occurence since we don't wanna replace the repo name.
                        string output = Regex.Replace(ext, @"(?<=templates.*)templates", "descriptions");
                        contents = await client.GetStringAsync(output);
                        remoteTemplateDescCache.Add(sel.Text, contents);
                    }
                    
                    TextDescription.Text = contents;
                }
                else
                {
                    string fullPathDesc = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                    , "Templates", sel.Text + ".txt"));
                    FileStream f = new(fullPathDesc, FileMode.Open);
                    StreamReader sr = new(f);
                    TextDescription.Text = sr.ReadLine();
                    f.Close();
                }
            }
            catch (FileNotFoundException ex)
            {
                TextDescription.Text = $"No description file for \"{sel.Text}\".";
                Logger.Error($"{TextDescription.Text} Reason:\n{ex}");
            }
            catch (Exception ex)
            {
                var exc = ex;
                var djfgklhdfg = exc;
                TextDescription.Text = $"Couldn't get description for \"{sel.Text}\".";
                Logger.Error($"{TextDescription.Text} Reason:\n{ex}");
            }
        }

        private void ListTemplates_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            SelectedPath = (ListTemplates.SelectedItem as DefS)?.FullPath;
            this.Close();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            SelectedPath = (ListTemplates.SelectedItem as DefS)?.FullPath;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
