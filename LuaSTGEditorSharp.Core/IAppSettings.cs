using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp
{
    public interface IAppSettings
    {
        bool UseFolderPacking { get; }
        bool IgnoreTHLibWarn { get; }
        string ZipExecutablePath { get; }
        string LuaSTGExecutablePath { get; }
        
        string EditorOutputName { get; }

        bool IsEXEPathSet { get; }

        string TempPath { get; }
        string SLDir { get; set; }
        bool SaveResMeta { get; }
        bool PackProj { get; }
        bool BatchPacking { get; }

        bool SpaceIndentation { get; }
        int IndentationSpaceLength { get;}
    }
}
