using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("playerdefine.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(0)]
    //[XmlType(TypeName = "BulletDefine")]
    public class PlayerDefine : TreeNode
    {
        [JsonConstructor]
        private PlayerDefine() : base() { }

        public PlayerDefine(DocumentData workSpaceData)
            : this(workSpaceData, "", "true", "", "") { }

        public PlayerDefine(DocumentData workSpaceData, string name, string auto, string dpname, string rpname)
            : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Name", name, this));
            attributes.Add(new AttrItem("Difficulty", difficulty, this, "objDifficulty"));
            */
            Name = name;
            Autoinc = auto;
            DisplayNam = dpname;
            ReplayNam = rpname;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Name")]
        //[DefaultValue("")]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Autoinclude")]
        //[DefaultValue("")]
        public string Autoinc
        {
            get => DoubleCheckAttr(1, "bool", "Auto-include").attrInput;
            set => DoubleCheckAttr(1, "bool", "Auto-include").attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Display name")]
        //[DefaultValue("")]
        public string DisplayNam
        {
            get => DoubleCheckAttr(2, name: "Display name").attrInput;
            set => DoubleCheckAttr(2, name: "Display name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Replay name")]
        //[DefaultValue("")]
        public string ReplayNam
        {
            get => DoubleCheckAttr(3, name: "Replay name").attrInput;
            set => DoubleCheckAttr(3, name: "Replay name").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "" + NonMacrolize(0) + "=Class(player_class)\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
            }
            if (NonMacrolize(1) == "true")
            {
                yield return "AddPlayerToPlayerList('" + Macrolize(2) + "','" + Macrolize(0) + "','" + Macrolize(3) + "')";
            }
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            return "Define player \"" + NonMacrolize(0) + "\", auto include is " + NonMacrolize(1) + "";
        }

        public override object Clone()
        {
            var n = new PlayerDefine(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override MetaInfo GetMeta()
        {
            return new PlayerDefineMetaInfo(this);
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            return messages;
        }
    }
}
