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

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("listcreate.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class ListCreate : TreeNode
    {
        [JsonConstructor]
        private ListCreate() : base() { }

        public ListCreate(DocumentData workSpaceData)
            : this(workSpaceData, "", "") { }

        public ListCreate(DocumentData workSpaceData, string name, string initContent)
            : base(workSpaceData)
        {
            Name = name;
            InitContent = initContent;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }
        [JsonIgnore, NodeAttribute]
        public string InitContent
        {
            get => DoubleCheckAttr(1, "code", "Initial Content").attrInput;
            set => DoubleCheckAttr(1, "code", "Initial Content").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string ar = Macrolize(1);
            if (!string.IsNullOrEmpty(ar))
                yield return sp + Macrolize(0) + " = " + "{" + Macrolize(1) + "}\n";
            else
                yield return sp + Macrolize(0) + " = {}\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Create a list of name \"{NonMacrolize(0)}\"";
        }

        public override object Clone()
        {
            var n = new ListCreate(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
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
