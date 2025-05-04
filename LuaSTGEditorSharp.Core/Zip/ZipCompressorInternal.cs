﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using Serilog;

namespace LuaSTGEditorSharp.Zip
{
    public class ZipCompressorInternal : ZipCompressor
    {
        private readonly string targetArchivePath;
        private FileStream targetArchiveFS;
        private ZipFile targetArchive;
        private static ILogger Logger = EditorLogging.ForContext("PlainCopy");

        public ZipCompressorInternal(string targetArchivePath)
        {
            Encoding utf8 = Encoding.UTF8;
            //ZipStrings.CodePage = utf8.CodePage;
            ZipStrings.UseUnicode = true;
            this.targetArchivePath = targetArchivePath;
        }

        public override void PackByDict(Dictionary<string, string> path, bool removeIfExists)
        {
            try
            {
                foreach (string s in PackByDictReporting(path, removeIfExists)) { }
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to pack files. Reason:\n{e}");
                System.Windows.MessageBox.Show($"Packaging failed.\n{e}");
            }
        }
        
        public override IEnumerable<string> PackByDictReporting(Dictionary<string, string> path, bool removeIfExists)
        {
            HashSet<string> zipNames = new HashSet<string>();
            try
            {
                if (File.Exists(targetArchivePath))
                {
                    if (removeIfExists)
                    {
                        File.Delete(targetArchivePath);
                        targetArchive = ZipFile.Create(targetArchivePath);
                    }
                    else
                    {
                        targetArchive = new ZipFile(targetArchivePath);
                    }
                }
                else
                {
                    targetArchive = ZipFile.Create(targetArchivePath);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Packaging failed. Reason:\n{e}");
                System.Windows.MessageBox.Show($"Packaging failed.\n{e}");
                yield break;
            }
            foreach (ZipEntry ze in targetArchive)
            {
                zipNames.Add(ze.Name);
                //System.Windows.MessageBox.Show(ze.Name);
            }
            foreach (KeyValuePair<string, string> kvp in path)
            {
                targetArchive.BeginUpdate();
                yield return $"Adding file \"{kvp.Value}\" in to zip.";
                if (targetArchive.FindEntry(kvp.Key, true) > 0)
                {
                    targetArchive.Delete(kvp.Key);
                }
                targetArchive.Add(kvp.Value, kvp.Key);
                yield return $"Added file \"{kvp.Value}\" in to zip.";
                targetArchive.CommitUpdate();
            }
            ((IDisposable)targetArchive).Dispose();
        }

        //Problem occurs when updating
        // Runa: Then why keep the code????
        [Obsolete]
        private IEnumerable<string> PackByDictReporting_old(Dictionary<string, string> path, bool removeIfExists)
        {
            HashSet<string> zipNames = new HashSet<string>();
            try
            {
                if (File.Exists(targetArchivePath))
                {
                    if (removeIfExists)
                    {
                        File.Delete(targetArchivePath);
                        targetArchiveFS = File.Create(targetArchivePath);
                    }
                    else
                    {
                        targetArchiveFS = new FileStream(targetArchivePath, FileMode.Open);
                    }
                }
                else
                {
                    targetArchiveFS = File.Create(targetArchivePath);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Packaging failed.\n{e}");
                yield break;
            }
            //targetArchiveFS.Close();
            //((IDisposable)targetArchiveFS).Dispose();

            using (ZipOutputStream ZipStream = new ZipOutputStream(targetArchiveFS))
            {
                foreach (KeyValuePair<string, string> kvp in path)
                {
                    using (FileStream StreamToZip = new FileStream(kvp.Value, FileMode.Open, FileAccess.Read))
                    {
                        yield return $"Add file \"{kvp.Value}\" into archive \"{targetArchivePath}\", internal name: \"{kvp.Key}\".";
                        ZipEntry ZipEntry = new ZipEntry(kvp.Key);

                        ZipStream.PutNextEntry(ZipEntry);
                        ZipStream.Flush();

                        byte[] buffer = new byte[2048];

                        int sizeRead = 0;

                        try
                        {
                            do
                            {
                                sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        StreamToZip.Close();
                    }
                }
                ZipStream.Finish();
                ZipStream.Close();
                yield return $"Finished archive \"{targetArchivePath}\".";
            }
            targetArchiveFS.Close();
        }

    }
}
