using IniParser;
using IniParser.Model;
using IniParser.Parser;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Compression;
using System.IO;

namespace LuaSTGEditorSharp.Addons;

public enum AddonType
{
    Preset,
    Node
}

public sealed class AddonObjectInfo
{
    public MetaAddonInfo Meta = new();

    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Don't use this for proper initialization alone. Only use with manual setting of <see cref="Meta"/>.
    /// </summary>
    public AddonObjectInfo() { }

    /// <summary>
    /// The correct initializer.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    public AddonObjectInfo(AddonType? type, KeyDataCollection data)
    {
        Meta = new(type, data)
        {
            RepoLink = GetRepoLink()
        };

        IsEnabled = bool.Parse(AddonManager.AddonConfigData[Meta.Name]["Enabled"]);
    }

    public string GetRepoLink()
    {
        return $"https://github.com/Sharp-X-Team/LuaSTG-Editor-Sharp-X/Addons/{Meta.Name}";
    }

    public static AddonObjectInfo FromMeta(MetaAddonInfo meta)
    {
        AddonObjectInfo addon = new()
        {
            Meta = meta
        };
        return addon;
    }

    public SectionData ToSectionData()
    {
        SectionData section = new(Meta.Name);
        section.Keys.AddKey("Type", Meta.Type.ToString());
        section.Keys.AddKey("PathToManifest", Path.Combine(Directory.GetCurrentDirectory(), $"Addons{Meta.FolderName}/manifest.ini"));
        section.Keys.AddKey("Enabled", IsEnabled.ToString());
        return section;
    }
}
