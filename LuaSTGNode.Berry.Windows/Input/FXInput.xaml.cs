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
    public partial class FXInput : InputWindow
    {
        ObservableCollection<MetaModel> allFxInfo;
        ObservableCollection<MetaModel> allFxInfoSys;

        ObservableCollection<MetaModel> filteredFxInfo;
        ObservableCollection<MetaModel> filteredFxInfoSys;

        public ObservableCollection<MetaModel> FilteredFxInfo { get => filteredFxInfo; }
        public ObservableCollection<MetaModel> FilteredFxInfoSys { get => filteredFxInfoSys; }

        public override string Result
        {
            get => result;
            set
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }

        public FXInput(string s, AttrItem item)
        {
            allFxInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.FXLoad].GetAllSimpleWithDifficulty();
            AddInternalMetas();
            filteredFxInfo = new ObservableCollection<MetaModel>(allFxInfo);
            filteredFxInfoSys = new ObservableCollection<MetaModel>(allFxInfoSys);

            InitializeComponent();

            Result = s;
            codeText.Text = Result;
        }

        private void Filter_TextChanged(object sender, RoutedEventArgs e)
        {
            filteredFxInfo.Clear();
            foreach (MetaModel mm in allFxInfo.Where(mm => MatchFilter(mm.FullName, filter.Text)))
            {
                filteredFxInfo.Add(mm);
            }
            filteredFxInfoSys.Clear();
            foreach (MetaModel mm in allFxInfoSys.Where(mm => MatchFilter(mm.FullName, filter.Text)))
            {
                filteredFxInfoSys.Add(mm);
            }
        }

        private void AddInternalMetas()
        {
            allFxInfoSys = new ObservableCollection<MetaModel>();
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
            if (m == null) return;
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
                txtPreview.Text = $"Failed To Load FX File \"{m?.ExInfo1}\".\n";
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
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                this.Close();
            }
        }
    }
}
