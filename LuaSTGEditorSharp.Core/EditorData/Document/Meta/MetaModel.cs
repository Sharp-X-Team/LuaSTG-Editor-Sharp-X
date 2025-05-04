﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    [Serializable]
    public class MetaModel : INotifyPropertyChanged, IAggregatableMeta, ICloneable
    {
        [NonSerialized]
        private string icon;
        [NonSerialized]
        private string text;

        [JsonProperty]
        public string Icon
        {
            get => icon;
            set
            {
                icon = value;
                RaisePropertyChanged("Icon");
            }
        }
        [JsonProperty]
        public string Text
        {
            get => text;
            set
            {
                text = value;
                RaisePropertyChanged("Text");
            }
        }
        [JsonProperty]
        public string Result { get; set; }
        [JsonProperty]
        public string Difficulty { get; set; }
        [JsonProperty]
        public string FullName { get; set; }
        [JsonProperty]
        public string Param { get; set; }
        [JsonProperty]
        public string ExInfo1 { get; set; }
        [JsonProperty]
        public string ExInfo2 { get; set; }

        public string GetDifficulty()
        {
            return Difficulty;
        }

        public string GetFullName()
        {
            return FullName;
        }

        public string GetParam()
        {
            return Param;
        }

        public string GetExInfo()
        {
            return Param;
        }

        [JsonProperty]
        public ObservableCollection<MetaModel> Children { get; set; } = new ObservableCollection<MetaModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public MetaModel GetSimpleMetaModel()
        {
            return (MetaModel)Clone();
        }

        public MetaModel GetFullMetaModel()
        {
            return GetSimpleMetaModel();
        }

        public object Clone()
        {
            return new MetaModel()
            {
                Icon = Icon,
                Text = Text,
                Result = Result,
                Difficulty = Difficulty,
                ExInfo1 = ExInfo1,
                FullName = FullName,
                ExInfo2 = ExInfo2,
                Param = Param,
                Children = new ObservableCollection<MetaModel>(from MetaModel m in Children select m)
            };
        }
    }
}
