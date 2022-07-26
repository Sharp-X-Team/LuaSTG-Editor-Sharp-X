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
    [Serializable, NodeIcon("itemdefine.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class DefineItem : TreeNode
    {
        [JsonConstructor]
        private DefineItem() : base() { }

        public DefineItem(DocumentData workSpaceData)
            : this(workSpaceData, "", "All") { }

        public DefineItem(DocumentData workSpaceData, string name, string difficulty)
            : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Name", name, this));
            attributes.Add(new AttrItem("Difficulty", difficulty, this, "objDifficulty"));
            */
            Name = name;
            Difficulty = difficulty;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Name")]
        //[DefaultValue("")]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Difficulty")]
        //[DefaultValue("All")]
        public string Difficulty
        {
            get => DoubleCheckAttr(1, "objDifficulty").attrInput;
            set => DoubleCheckAttr(1, "objDifficulty").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "_editor_class[\"" + Lua.StringParser.ParseLua(NonMacrolize(0)) + "\"]=Class(_object)\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
            }
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            return "Define item \"" + NonMacrolize(0) + "\"";
        }

        public override object Clone()
        {
            var n = new DefineItem(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override MetaInfo GetMeta()
        {
            return new ItemDefineMetaInfo(this);
        }

        public override string GetDifficulty()
        {
            return "";
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
