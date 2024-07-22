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
    public abstract class AddonObjectInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RepoLink { get; set; }

        public string GetRepoLink()
        {
            return $"https://github.com/Sharp-X-Team/LuaSTG-Editor-Sharp-X/Addons/{Name}";
        }

        public bool DownloadAddonFiles()
        {
            return true;
        }
    }
}
