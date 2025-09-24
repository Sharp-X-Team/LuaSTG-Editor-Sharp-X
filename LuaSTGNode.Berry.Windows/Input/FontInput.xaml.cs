﻿using System;
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
    public partial class FontInput : InputWindow
    {
        public ObservableCollection<MetaModel> fontInfo;
        public ObservableCollection<MetaModel> FontInfo { get => fontInfo; }

        public ObservableCollection<MetaModel> fontInfoSys;
        public ObservableCollection<MetaModel> FontInfoSys { get => fontInfoSys; }

        public override string Result
        {
            get => result;
            set
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }

        public FontInput(string s, AttrItem item)
        {
            fontInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.FontLoad].GetAllSimpleWithDifficulty();

            AddInternalMetas();

            InitializeComponent();

            Result = s;
            codeText.Text = Result;
        }

        private void AddInternalMetas()
        {
            fontInfoSys = new ObservableCollection<MetaModel>();
            fontInfoSys = new ObservableCollection<MetaModel>(NodesConfig.SysFont);
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
                Uri uri = new Uri(m?.ExInfo1);
                if (uri.Scheme == "file")
                {
                    sr = new StreamReader(m?.ExInfo1);
                }
                else
                {
                    StreamResourceInfo info = Application.GetResourceStream(uri);
                    sr = new StreamReader(info.Stream);
                }
                txtPreview.Text = sr.ReadToEnd();
            }
            catch
            {
                txtPreview.Text = $"Failed To Load Font File \"{m?.ExInfo1}\".\n";
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
