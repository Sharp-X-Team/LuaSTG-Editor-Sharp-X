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
    [Serializable, NodeIcon("bgstagecreate.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(0)]
    //[XmlType(TypeName = "BulletDefine")]
    public class BackgroundDefine : TreeNode
    {
        [JsonConstructor]
        private BackgroundDefine() : base() { }

        public BackgroundDefine(DocumentData workSpaceData)
            : this(workSpaceData, "") { }

        public BackgroundDefine(DocumentData workSpaceData, string name)
            : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Name", name, this));
            attributes.Add(new AttrItem("Difficulty", difficulty, this, "objDifficulty"));
            */
            Name = name;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Name")]
        //[DefaultValue("")]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"_editor_class[\"{Lua.StringParser.ParseLua(NonMacrolize(0))}\"] = Class(_object)\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
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
            return "Define background \"" + NonMacrolize(0) + "\"";
        }

        public override object Clone()
        {
            var n = new BackgroundDefine(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override MetaInfo GetMeta()
        {
            return new BackgroundDefineMetaInfo(this);
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = [];
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            return messages;
        }
    }
}
