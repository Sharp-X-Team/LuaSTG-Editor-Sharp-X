using IniParser.Model;
using IniParser.Parser;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LuaSTGEditorSharp.Addons;

public class MetaAddonInfo
{
    public MetaAddonInfo() { }

    public MetaAddonInfo(AddonType? type, KeyDataCollection data)
    {
        Type = type;
        Name = data["Name"];
        Description = data["Description"];
        Author = data["Owner"];
        ChangeLog = data["Changelog"];
        Version = int.Parse(data["Version"]);
        PathToIcon = data["Icon"];
    }

    public AddonType? Type { get; set; }
    public string Name { get; set; }
    public string FolderName { get; set; }
    public string Description { get; set; }
    public string PathToIcon { get; set; } = string.Empty;
    public string Author { get; set; }
    public string ChangeLog { get; set; }
    public string RepoLink { get; set; }
    public int Version { get; set; }

    public static async Task<MetaAddonInfo?> ExtractManifest(ZipArchive zipArc, string AddonFolder)
    {
        try
        {
            string manifestPath = string.Empty;
            string firstEntryName = GetFirstFolderName(zipArc);
            string extractPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), $"lstges/Addons{AddonFolder}"));
            string folderPath = Path.Combine(firstEntryName, $"Addons{AddonFolder}");
            foreach (ZipArchiveEntry entry in zipArc.Entries)
            {
                if (entry.FullName.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase))
                {
                    // Determine the destination path
                    string relativePath = entry.FullName.Substring(folderPath.Length);
                    string destinationPath = Path.Combine(extractPath, relativePath);

                    if (entry.FullName.EndsWith("/")) // Directory
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    else // File
                    {
                        if (!entry.FullName.Contains("manifest.ini"))
                            continue;
                        // Ensure the directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                        entry.ExtractToFile(destinationPath, overwrite: true);
                        if (string.IsNullOrEmpty(manifestPath))
                            manifestPath = destinationPath;
                    }
                }
            }

            string manifestContent = File.ReadAllText(manifestPath);
            IniDataParser parser = new();
            IniData data = parser.Parse(manifestContent);

            if (data["node"].Count != 0)
                return new(AddonType.Node, data["node"]);
            else
                return new(AddonType.Preset, data["preset"]);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
            return null;
        }
    }

    public async Task<bool> ExtractAddonFiles(ZipArchive zipArc)
    {
        try
        {
            string firstEntryName = GetFirstFolderName(zipArc);
            string extractPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), $"Addons{FolderName}"));
            string folderPath = Path.Combine(firstEntryName, $"Addons{FolderName}");
            foreach (ZipArchiveEntry entry in zipArc.Entries)
            {
                if (entry.FullName.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase))
                {
                    // Determine the destination path
                    string relativePath = entry.FullName.Substring(folderPath.Length);
                    string destinationPath = Path.Combine(extractPath, relativePath);

                    if (entry.FullName.EndsWith("/")) // Directory
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    else // File
                    {
                        // Ensure the directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                        entry.ExtractToFile(destinationPath, overwrite: true);
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Something went wrong during addon install.\n{ex}");
            return false;
        }
    }

    public static string GetFirstFolderName(ZipArchive zipArc)
    {
        string firstEntryName = zipArc.Entries[0].FullName;
        int firstSlashIndex = firstEntryName.IndexOf('/');
        if (firstSlashIndex > -1)
            firstEntryName = firstEntryName.Substring(0, firstSlashIndex + 1);

        return firstEntryName;
    }
}