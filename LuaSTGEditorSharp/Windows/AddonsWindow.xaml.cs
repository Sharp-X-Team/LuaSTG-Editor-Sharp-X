using IniParser.Model;
using IniParser.Parser;
using LuaSTGEditorSharp.Addons;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Specialized;
using System.IO;
using IniParser;

namespace LuaSTGEditorSharp.Windows
{
    public partial class AddonsWindow : Window, INotifyPropertyChanged
    {
        public static readonly string PathToConfig = $"{Directory.GetCurrentDirectory()}/Addons/AddonsConfig.ini";
        public IniData AddonConfigData;
        public bool AreAddonsEnabled
        {
            get => bool.Parse(AddonConfigData["Core_Common"]["EnableAddons"]);
            set => AddonConfigData["Core_Common"]["EnableAddons"] = value.ToString();
        }

        public string SelectedToInstall => $"Install ({AddonsToDownload.Count})"; // temp
        public string SelectedToUninstall => $"Uninstall ({0})"; // temp

        private ObservableCollection<AddonObjectInfo> listPresets = new ObservableCollection<AddonObjectInfo>();
        public ObservableCollection<AddonObjectInfo> ListPresets
        {
            get => listPresets;
            set
            {
                if (value == null)
                {
                    listPresets = new ObservableCollection<AddonObjectInfo>();
                    //listPresets.CollectionChanged += new NotifyCollectionChangedEventHandler(this.AttributesChanged);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private ObservableCollection<AddonObjectInfo> listNodes = new ObservableCollection<AddonObjectInfo>();
        public ObservableCollection<AddonObjectInfo> ListNodes
        {
            get => listNodes;
            set
            {
                if (value == null)
                {
                    listNodes = new ObservableCollection<AddonObjectInfo>();
                    //listNodes.CollectionChanged += new NotifyCollectionChangedEventHandler(this.AttributesChanged);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public List<AddonObjectInfo> InstalledAddons = new List<AddonObjectInfo>();
        public List<AddonObjectInfo> AddonsToDownload = new List<AddonObjectInfo>();

        // Help
        public AddonsWindow()
        {
            ListPresets = null;
            ListNodes = null;
            bool initialized = false;
            if (TryReadConfig())
            {
                TryGetAddonsList();
                initialized = true;
            }
            InitializeComponent();
            if (!initialized)
                TabControls.IsEnabled = false;
        }

        public AddonsWindow(int i)
            : this()
        {
            switch (i)
            {
                case 0:
                default:
                    AddonsTab.IsSelected = true;
                    break;
                case 1:
                    PresetTab.IsSelected = true;
                    break;
                case 2:
                    NodesTab.IsSelected = true;
                    break;
            }
        }

        #region Install

        public void InstallSelected(AddonObjectInfo selected, bool installingBulk = false)
        {
            SectionData newSection = selected.ToSectionData();
            AddonConfigData.Sections.Add(newSection);

            // Avoid possible file access violations. If installing in bulk, doesn't save until all addons are installed.
            if (!installingBulk)
            {
                FileIniDataParser parser = new FileIniDataParser();
                parser.WriteFile(PathToConfig, AddonConfigData);
            }
        }

        public void InstallAllSelected()
        {
            foreach (AddonObjectInfo addon in AddonsToDownload)
                InstallSelected(addon, true);
            FileIniDataParser parser = new FileIniDataParser();
            parser.WriteFile(PathToConfig, AddonConfigData);
        }

        #endregion
        #region Uninstall

        public void UninstallSelected(AddonObjectInfo selected)
        {

        }

        public void UninstallAllSelected()
        {
            //foreach (AddonObjectInfo addon in AddonsToUninstall)
            //    UninstallSelected(addon);
        }

        #endregion
        #region Manage Addons

        public bool TryReadConfig()
        {
            try
            {
                if (!File.Exists(PathToConfig))
                    return false; // Something is very wrong here

                FileIniDataParser parser = new FileIniDataParser();
                AddonConfigData = parser.ReadFile(PathToConfig);
                if (!bool.Parse(AddonConfigData["Core_Common"]["EnableAddons"]))
                    return false; // Addons aren't enabled, skipping.

                foreach (SectionData section in AddonConfigData.Sections)
                {
                    if (section.SectionName == "Core_Common")
                        continue; // This is definitely not a fucking addon.
                    AddonObjectInfo addon = new AddonObjectInfo(section.Keys);
                    InstalledAddons.Add(addon);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was a problem trying to read AddonsConfig.ini. Aborting initialization.\nError: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Try to find an update for a given addon.
        /// </summary>
        /// <returns>True if update is found; false if there is no updates.</returns>
        public bool TryFindUpdate()
        {
            return false;
        }

        #endregion
        #region TryGets

        private async Task TryGetAddonsList()
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.UserAgent.Add(
                    new ProductInfoHeaderValue("LuaSTG_Editor_Sharp_X", "1"));
                var repo = "Sharp-X-Team/Sharp-X-Addons";
                var contentsUrl = $"https://api.github.com/repos/{repo}/contents/Addons";
                var contentsJson = await httpClient.GetStringAsync(contentsUrl);
                var contents = (JArray)JsonConvert.DeserializeObject(contentsJson);
                foreach (var item in contents)
                {
                    AddonObjectInfo addonInfo = GetAddonInfo((string)item["name"]);
                    if ((string)item["type"] != "dir" || addonInfo == null)
                        continue;
                    if (addonInfo.Type == AddonType.Node)
                        listNodes.Add(addonInfo);
                    else if (addonInfo.Type == AddonType.Preset)
                        listPresets.Add(addonInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private AddonObjectInfo GetAddonInfo(string folderPath)
        {
            string linkToManifest = $"https://raw.githubusercontent.com/Sharp-X-Team/Sharp-X-Addons/main/Addons/{folderPath}/manifest.ini";
            try
            {
                string manifestContent;
                using (WebClient client = new WebClient())
                    manifestContent = client.DownloadString(linkToManifest);
                IniDataParser parser = new IniDataParser();
                IniData data = parser.Parse(manifestContent);

                AddonObjectInfo addon = null;
                if (data["node"].Count != 0)
                    addon = new AddonObjectInfo(AddonType.Node, data["node"]);
                else
                    addon = new AddonObjectInfo(AddonType.Preset, data["preset"]);
                return addon;
            }
            catch
            {
                return null;
            }
        }

        #endregion
        #region Events

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null)
            {
                foreach (var item in e.AddedItems)
                    AddonsToDownload.Add(item as AddonObjectInfo);
            }
            if (e.RemovedItems != null)
            {
                foreach (var item in e.RemovedItems)
                    AddonsToDownload.Remove(item as AddonObjectInfo);
            }
            RaisePropertyChanged("AddonsToDownload");
            RaisePropertyChanged("SelectedToInstall");
        }

        #endregion
        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propName = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }
}
