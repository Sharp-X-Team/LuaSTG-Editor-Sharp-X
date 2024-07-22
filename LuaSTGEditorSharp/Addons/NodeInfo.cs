using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Addons
{
    public class NodeInfo : AddonObjectInfo
    {
        public NodeInfo(KeyDataCollection data)
        {
            Name = data["Name"];
            Description = data["Description"];
            RepoLink = GetRepoLink();
        }
    }
}
