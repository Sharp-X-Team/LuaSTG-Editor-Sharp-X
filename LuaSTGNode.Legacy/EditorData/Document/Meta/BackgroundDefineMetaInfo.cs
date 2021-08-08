using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Node.Bullet;
using LuaSTGEditorSharp.EditorData.Node.Object;
using LuaSTGEditorSharp.Windows;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public class BackgroundDefineMetaInfo : MetaInfo, IComparable<BackgroundDefineMetaInfo>
    {
        BackgroundInit Init { get; set; }

        public BackgroundDefineMetaInfo(BackgroundDefine target) : base(target) { }

        private void TryChild()
        {
            foreach (TreeNode t in this.target.GetLogicalChildren())
            {
                if (t is BackgroundInit) Init = t as BackgroundInit;
            }
        }

        public override string Name
        {
            get => Lua.StringParser.ParseLua(target.attributes[0].AttrInput);
        }

        public string Params
        {
            get
            {
                TryChild();
                return Init.attributes[0].AttrInput;
            }
        }

        public string[] GetParamList()
        {
            TryChild();
            string s = Init.attributes[0].AttrInput;
            if(!string.IsNullOrEmpty(s))
            {
                return s.Split(',');
            }
            else
            {
                return new string[] { };
            }
        }

        public string[] GetCallBackFunc()
        {
            return (from TreeNode t 
                    in target.GetLogicalChildren()
                    where t is CallBackFunc
                    select t.attributes[0].AttrInput).ToArray();
        }

        public int CompareTo(BackgroundDefineMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.Background].Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.Background].Remove(meta);
        }

        public override string FullName
        {
            get
            {
                return Name;
            }
        }

        public override string ScrString
        {
            get => "Name: " + Name + "\nParameters: " + Params;
        }

        public override string Difficulty => throw new NotImplementedException();

        public override MetaModel GetFullMetaModel()
        {
            MetaModel parent = new MetaModel
            {
                Icon = "/LuaSTGNode.Legacy;component/images/16x16/bgstagecreate.png",
                Text = Name
            };
            MetaModel child = new MetaModel()
            {
                Icon = "/LuaSTGNode.Legacy;component/images/16x16/bgstagecreate.png",
                Text = "init"
            };
            parent.Children.Add(child);
            string[] childs = GetParamList();
            MetaModel prop;
            foreach (string s in childs)
            {
                prop = new MetaModel
                {
                    Icon = "/LuaSTGEditorSharp.Core;component/images/16x16/properties.png",
                    Text = s
                };
                child.Children.Add(prop);
            }
            childs = GetCallBackFunc();
            foreach (string s in childs)
            {
                child = new MetaModel
                {
                    Icon = "/LuaSTGNode.Legacy;component/images/16x16/callbackfunc.png",
                    Text = s
                };
                parent.Children.Add(child);
            }
            return parent;
        }

        public override MetaModel GetSimpleMetaModel()
        {
            return new MetaModel
            {
                Result = "\"" + FullName + "\"",
                Text = ScrString,
                FullName = FullName,
                Param = Params,
                Icon = "/LuaSTGNode.Legacy;component/images/16x16/bgstagecreate.png"
            };
        }

        public override string GetParam()
        {
            return Params;
        }
    }
}
