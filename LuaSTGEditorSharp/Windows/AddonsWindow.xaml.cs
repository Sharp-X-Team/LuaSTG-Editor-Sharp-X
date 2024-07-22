using IniParser.Model;
using IniParser.Parser;
using LuaSTGEditorSharp.Addons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LuaSTGEditorSharp.Windows
{
    public partial class AddonsWindow : Window, INotifyPropertyChanged
    {
        public string SelectedToInstall => $"Install ({0})"; // temp
        public string SelectedToUninstall => $"Uninstall ({0})"; // temp

        private ObservableCollection<string> listPreset;
        public ObservableCollection<string> ListPreset
        {
            get => listPreset;
            set
            {
                listPreset = value;
                RaisePropertyChanged();
            }
        }

        public AddonsWindow()
        {
            TryGetPresets();
            InitializeComponent();
        }

        public AddonsWindow(int i)
            : this()
        {
            switch (i)
            {
                case 0:
                default:
                    PresetTab.IsSelected = true;
                    break;
                case 1:
                    NodesTab.IsSelected = true;
                    break;
            }
        }

        #region TryGets

        private void TryGetPresets()
        {
            try
            {

            }
            catch
            {

            }
        }

        private AddonObjectInfo GetAddon(string folderName)
        {
            string linkToManifest = $"https://raw.githubusercontent.com/Sharp-X-Team/Sharp-X-Addons/main/Addons/{Name}/manifest.ini";
            try
            {
                string manifestContent;
                using (WebClient client = new WebClient())
                    manifestContent = client.DownloadString(linkToManifest);
                IniDataParser parser = new IniDataParser();
                IniData data = parser.Parse(manifestContent);

                AddonObjectInfo addon = null;
                if (data["node"] != null)
                    addon = new NodeInfo(data["node"]);
                else
                    addon = new PresetInfo(data["preset"]);
                return addon;
            }
            catch
            {
                return null;
            }
        }

        #endregion
        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propName = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #endregion

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PresetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
