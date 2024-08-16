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
using System.Collections.Specialized;
using System.IO;
using IniParser;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace LuaSTGEditorSharp.Windows;

public partial class AddonsWindow : Window, INotifyPropertyChanged
{
    public static readonly string PathToZip = $"{Directory.GetCurrentDirectory()}/Addons/AddonList.zip";
    
    public bool AreAddonsEnabled
    {
        get => bool.Parse(AddonManager.AddonConfigData["Core_Common"]["EnableAddons"]);
        set => AddonManager.AddonConfigData["Core_Common"]["EnableAddons"] = value.ToString();
    }

    public string SelectedToInstall => $"Install ({AddonsToDownload.Count})";
    public string SelectedToUninstall => $"Uninstall ({AddonsToUninstall.Count})";

    public ObservableCollection<MetaAddonInfo> allAddonList;

    private ObservableCollection<MetaAddonInfo> _listPresets;
    public ObservableCollection<MetaAddonInfo> ListPresets
    {
        get => _listPresets;
        set
        {
            _listPresets = value;
            RaisePropertyChanged("ListPresets");
        }
    }

    private ObservableCollection<MetaAddonInfo> _listNodes;
    public ObservableCollection<MetaAddonInfo> ListNodes
    {
        get => _listNodes;
        set
        {
            _listNodes = value;
            RaisePropertyChanged("ListNodes");
        }
    }

    public ObservableCollection<MetaAddonInfo> AddonsToDownload { get; set; } = [];
    public ObservableCollection<MetaAddonInfo> AddonsToUninstall { get; set; } = [];

    // Warning: Fuckery. I don't want to use too much memory, so I have to do this. Sorry lmao <3
    // (But I did end up creating two Lists inside of here each call so uhhhh that may defeat the purpose?? If it works, it works ¯\_(ツ)_/¯.
    private ObservableCollection<MetaAddonInfo> GetNodeAddons(AddonType type, ObservableCollection<MetaAddonInfo> queryList)
    {
        List<string> existingNames = [];
        foreach (var item in AddonManager.InstalledAddons)
            existingNames.Add(item.Meta.Name);
        // This will check if the type is the matching one and if the addon is already installed.
        List<MetaAddonInfo> filteredList = queryList.Where(a => a.Type == type && !existingNames.Contains(a.Name)).ToList();
        return new ObservableCollection<MetaAddonInfo>(filteredList);
    }

    // Help
    public AddonsWindow()
    {
        allAddonList = [];
        bool initialized = false;
        if (AddonManager.TryLoadAddons())
        {
#pragma warning disable CS4014
            TryGetAddonsList();
#pragma warning restore CS4014
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

    /// <summary>
    /// This ideally should not be used to install multiple addons at once but inside <see cref="InstallAllSelected"/> bulk method.<br/>
    /// This is because the bulk method creates a single zip instance for all of this method's calls.<br/>
    /// I'm not even sure if calling it by itself is safe.
    /// </summary>
    /// <param name="selected"></param>
    /// <param name="zipArc"></param>
    public async Task<bool> InstallSelected(MetaAddonInfo selected, ZipArchive zipArc = null)
    {
        bool installingBulk = false;
        if (zipArc == null)
            installingBulk = true;

        using (zipArc ??= ZipFile.OpenRead(PathToZip))
        {
            if (!await selected.ExtractAddonFiles(zipArc))
                return false; // Something went wrong.
            AddonObjectInfo addon = AddonObjectInfo.FromMeta(selected);
            SectionData newSection = addon.ToSectionData();
            AddonManager.AddonConfigData.Sections.Add(newSection);

            // Avoid possible file access violations. If installing in bulk, doesn't save until all addons are installed.
            // This is very probably useless, but we never know what can happen with file access.
            if (!installingBulk)
            {
                FileIniDataParser parser = new();
                parser.WriteFile(AddonManager.PathToConfig, AddonManager.AddonConfigData);
                ListPresets = GetNodeAddons(AddonType.Preset, allAddonList);
                ListNodes = GetNodeAddons(AddonType.Node, allAddonList);
            }
            return true;
        }
    }

    /// <summary>
    /// This is the prefered way to install addons.<br/>
    /// Calls <see cref="InstallSelected(MetaAddonInfo, ZipArchive)"/> with an existing <see cref="ZipArchive"/> instance.
    /// </summary>
    public async Task InstallAllSelected()
    {
        int addonsInstalledCount = 0;
        int addonsToInstallCount = AddonsToDownload.Count;
        using (ZipArchive zipArc = ZipFile.OpenRead(PathToZip))
        {
            foreach (MetaAddonInfo addon in AddonsToDownload)
                if (await InstallSelected(addon, zipArc))
                    addonsInstalledCount++;
        }
        FileIniDataParser parser = new();
        parser.WriteFile(AddonManager.PathToConfig, AddonManager.AddonConfigData);
        ListPresets = GetNodeAddons(AddonType.Preset, allAddonList);
        ListNodes = GetNodeAddons(AddonType.Node, allAddonList);

        MessageBox.Show($"Installed {addonsInstalledCount} of {addonsToInstallCount} addons!" +
            ((addonsInstalledCount != addonsToInstallCount) ? $"\nThere was a problem installing {addonsToInstallCount - addonsInstalledCount} addons." : ""));
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
        IsEnabled = false;
        if (!await DownloadAddonZip()) // help
            return;

        try
        {
            using (ZipArchive arc = ZipFile.OpenRead(PathToZip))
            {
                allAddonList.Clear();
                // Extract manifest and create AddonObjectInfo.
                string baseFolderPath = Path.Combine(MetaAddonInfo.GetFirstFolderName(arc), "Addons");
                foreach (ZipArchiveEntry entry in arc.Entries)
                {
                    
                    if (entry.FullName.StartsWith(baseFolderPath, StringComparison.OrdinalIgnoreCase))
                    {
                        string relativePath = entry.FullName.Substring(baseFolderPath.Length);
                        if (!Regex.IsMatch(relativePath, @"^/[^/]+/$"))
                            continue;
                        MetaAddonInfo? addon = await MetaAddonInfo.ExtractManifest(arc, relativePath);
                        if (addon == null)
                            continue;
                        addon.FolderName = relativePath;
                        allAddonList.Add(addon);
                    }
                }
                ListPresets = GetNodeAddons(AddonType.Preset, allAddonList);
                ListNodes = GetNodeAddons(AddonType.Node, allAddonList);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }

        IsEnabled = true;
    }

    private async Task<bool> DownloadAddonZip()
    {
        // Skip downloading the addon zip if this window has already been opened on this SharpX instance (=launch).
        // Return true if the zip already exists.
        if (!AddonManager.DownloadUpdatedListOnOpen)
            return File.Exists(PathToZip);

        string url = $"https://api.github.com/repos/Sharp-X-Team/Sharp-X-Addons/zipball/main";

        try
        {
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                byte[] zipData = await response.Content.ReadAsByteArrayAsync();
                using (FileStream fs = new(PathToZip, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                {
                    await fs.WriteAsync(zipData, 0, zipData.Length);
                }
                // Should be called only once and only here. Will not update this zip if the window is re-opened on the same SharpX instance (=launch).
                AddonManager.DownloadUpdatedListOnOpen = false;
                return true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Something went wrong with refreshing the addon list.\n{ex}");
            return false;
        }
    }

    #endregion
    #region Events

    private async void DownloadAddonZip_Click(object sender, RoutedEventArgs e)
    {
        IsEnabled = false;
        await TryGetAddonsList();
        IsEnabled = true;
    }

    private async void DownloadSelectedAddons_Click(object sender, RoutedEventArgs e)
    {
        await InstallAllSelected();
    }

    private void ButtonClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems != null)
        {
            foreach (var item in e.AddedItems)
                AddonsToDownload.Add((MetaAddonInfo)item);
        }
        if (e.RemovedItems != null)
        {
            foreach (var item in e.RemovedItems)
                AddonsToDownload.Remove((MetaAddonInfo)item);
        }
        RaisePropertyChanged("AddonsToDownload");
        RaisePropertyChanged("SelectedToInstall");
    }

    #endregion
    #region Property Changed

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string propName = default!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    #endregion
}
