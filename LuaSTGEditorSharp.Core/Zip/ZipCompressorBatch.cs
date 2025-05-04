using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace LuaSTGEditorSharp.Zip
{
    public class ZipCompressorBatch : ZipCompressor
    {
        private readonly string zipExePath;
        private readonly string targetArchivePath;
        private readonly string batchTempPath;
        private static ILogger Logger = EditorLogging.ForContext("PlainCopy");

        public ZipCompressorBatch(string targetArchivePath, string zipExePath, string batchTempPath)
        {
            this.targetArchivePath = targetArchivePath;
            this.zipExePath = zipExePath;
            this.batchTempPath = batchTempPath;
        }

        public override void PackByDict(Dictionary<string, string> fileInfo, bool removeIfExists)
        {
            if (removeIfExists && File.Exists(targetArchivePath)) File.Delete(targetArchivePath);
            try
            {
                using FileStream packBatS = new(batchTempPath, FileMode.Create);
                using StreamWriter packBat = new(packBatS, Encoding.Default);
                foreach (KeyValuePair<string, string> kvp in fileInfo)
                {
                    packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetArchivePath + "\" \"" + kvp.Value + "\"");
                }
                packBat.Close();
                packBatS.Close();
                Process pack = new()
                {
                    StartInfo = new ProcessStartInfo(batchTempPath)
                    {
                        UseShellExecute = true,
                        CreateNoWindow = false
                    }
                };
                pack.Start();
                pack.WaitForExit();
            }
            catch (System.Exception e)
            {
                Logger.Error($"Failed to pack files. Reason:\n{e}");
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
