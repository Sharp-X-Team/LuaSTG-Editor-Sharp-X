using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Zip
{
    public class PlainCopy : ZipCompressor
    {
        private readonly string targetArchivePath;

        public PlainCopy(string targetArchivePath)
        {
            if (targetArchivePath.EndsWith(".zip"))
            {
                targetArchivePath = targetArchivePath.Substring(0, targetArchivePath.Length - 4);
            }
            this.targetArchivePath = targetArchivePath;
        }

        public override void PackByDict(Dictionary<string, string> fileInfo, bool removeIfExists)
        {
            if (removeIfExists && Directory.Exists(targetArchivePath)) { Directory.Delete(targetArchivePath, true); }
            if (!Directory.Exists(targetArchivePath)) { 
                Directory.CreateDirectory(targetArchivePath);
            }
            try
            {
                foreach (KeyValuePair<string, string> kvp in fileInfo)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(targetArchivePath + "\\" + kvp.Key));
                    File.Copy(kvp.Value, targetArchivePath + "\\" + kvp.Key, true);
                }
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }
        }

        public override IEnumerable<string> PackByDictReporting(Dictionary<string, string> path, bool removeIfExists)
        {
            PackByDict(path, removeIfExists);
            yield break;
        }
    }
}
