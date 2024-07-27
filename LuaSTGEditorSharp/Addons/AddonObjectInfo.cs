using IniParser;
using IniParser.Model;
using IniParser.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Addons
{
    public enum AddonType
    {
        Preset,
        Node
    }

    public sealed class AddonObjectInfo
    {
        public AddonType? Type { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string PathToIcon { get; set; } = string.Empty;
        public string Author { get; set; }
        public string ChangeLog { get; set; }
        public string RepoLink { get; set; }
        public int Version { get; set; }

        public bool IsInstalled { get => false; }
        public bool CanInstall { get => true; }

        public bool IsEnabled { get; set; }

        public AddonObjectInfo(AddonType? type, KeyDataCollection data)
        {
            Type = type;
            Name = data["Name"];
            Description = data["Description"];
            Author = data["Owner"];
            ChangeLog = data["Changelog"];
            Version = int.Parse(data["Version"]);
            PathToIcon = data["Icon"];

            RepoLink = GetRepoLink();
        }

        public AddonObjectInfo(KeyDataCollection data)
            : this(null, data) { }

        public string GetRepoLink()
        {
            return $"https://github.com/Sharp-X-Team/LuaSTG-Editor-Sharp-X/Addons/{Name}";
        }

        public SectionData ToSectionData()
        {
            SectionData section = new SectionData(Name);
            section.Keys.AddKey("Description", Description);
            section.Keys.AddKey("CurrentVersion", Version.ToString());
            section.Keys.AddKey("Author", Author);
            section.Keys.AddKey("Icon", PathToIcon);
            return section;
        }

        public bool DownloadAddonFiles()
        {
            return true;
        }
    }
}
