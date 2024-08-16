using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LuaSTGEditorSharp.Addons;

public static class AddonManager
{
    public static readonly string PathToConfig = $"{Directory.GetCurrentDirectory()}/Addons/AddonsConfig.ini";
    public static bool AreAddonsEnabled { get => ReadAreAddonsEnabled(); }

    public static IniData AddonConfigData { get; set; }

    public static ObservableCollection<AddonObjectInfo> InstalledAddons { get; set; } = [];

    // Warning: Don't set this manually outside of the indended usage on the AddonsWindow.
    public static bool DownloadUpdatedListOnOpen { get; set; } = true;

    public static bool ReadAreAddonsEnabled()
    {
        try
        {
            if (!File.Exists(PathToConfig))
                return false; // Something is very wrong here ಠ_ಠ

            FileIniDataParser parser = new();
            AddonConfigData = parser.ReadFile(PathToConfig);
            return bool.Parse(AddonConfigData["Core_Common"]["EnableAddons"]);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
            return false;
        }
    }

    // TODO: Have multiple custom errors for when the addon doesn't exist in the specified file access
    public static bool TryLoadAddons()
    {
        try
        {
            if (!AreAddonsEnabled)
                return false;

            foreach (SectionData section in AddonConfigData.Sections)
            {
                if (section.SectionName == "Core_Common")
                    continue; // This is definitely not a fucking addon. Skip.
                FileIniDataParser addonParse = new();
                IniData content = addonParse.ReadFile(AddonConfigData[section.SectionName]["PathToManifest"]);

                if (content["node"].Count != 0)
                    InstalledAddons.Add(new AddonObjectInfo(AddonType.Node, content["node"]));
                else
                    InstalledAddons.Add(new AddonObjectInfo(AddonType.Preset, content["preset"]));
            }
            BuildAddons();
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"There was a problem trying to read AddonsConfig.ini. Aborting initialization.\nError: {ex}");
            return false;
        }
    }

    // This is the method called after loading addons. This will add nodes to the toolboxes and presets to the preset list.
    private static void BuildAddons()
    {
        // Foreach addon, check the type and call the corresponding method on AddonObjectInfo.<whatever> to "build" them.
        return;
    }
}
