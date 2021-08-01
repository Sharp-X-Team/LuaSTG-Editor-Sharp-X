using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// ImageInput.xaml 的交互逻辑
    /// </summary>
    public partial class TTFInput : InputWindow
    {
        public ObservableCollection<MetaModel> ttfInfo;
        public ObservableCollection<MetaModel> TTFInfo { get => ttfInfo; }

        public ObservableCollection<MetaModel> ttfInfoSys;
        public ObservableCollection<MetaModel> TTFInfoSys { get => ttfInfoSys; }

        public override string Result
        {
            get => result;
            set
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }

        public TTFInput(string s, AttrItem item)
        {
            ttfInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.TTFLoad].GetAllSimpleWithDifficulty();

            AddInternalMetas();

            InitializeComponent();

            Result = s;
            codeText.Text = Result;
        }

        private void AddInternalMetas()
        {
            ttfInfoSys = new ObservableCollection<MetaModel>();
            ttfInfoSys = new ObservableCollection<MetaModel>(NodesConfig.SysTTF);
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void InputWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(codeText);
        }

        private void BoxSEData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MetaModel m = (BoxSEData.SelectedItem as MetaModel);
            if (!string.IsNullOrEmpty(m?.Result)) Result = m?.Result;

            StreamReader sr = null;
            try
            {
                txtPreview.Text = $"Archive preview disabled for TTF \"{m?.ExInfo1}\".\n";
            }
            catch
            {
                txtPreview.Text = $"Archive preview disabled for TTF \"{m?.ExInfo1}\".\n";
            }
            finally
            {
                if (sr != null) sr.Close();
            }
            codeText.Focus();
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void Text_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                DialogResult = true;
                this.Close();
            }
        }
    }
}
