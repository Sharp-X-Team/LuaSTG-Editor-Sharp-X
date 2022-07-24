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
    [Serializable, NodeIcon("listadd.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class ListRemove : TreeNode
    {
        [JsonConstructor]
        private ListRemove() : base() { }

        public ListRemove(DocumentData workSpaceData)
            : this(workSpaceData, "", "last") { }

        public ListRemove(DocumentData workSpaceData, string name, string position)
            : base(workSpaceData)
        {
            Name = name;
            Position = position;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string ar = Macrolize(1);
            if (ar == "last")
                yield return sp + $"table.remove({Macrolize(0)})\n";
            else
                yield return sp + $"table.remove({Macrolize(0)}, {Macrolize(1)})\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            string returnstring;
            if (NonMacrolize(1) != "last")
                returnstring = "Remove value from list " + NonMacrolize(0) + $" at position {NonMacrolize(1)}";
            else
                returnstring = "Remove value from list " + NonMacrolize(0) + " at the last position";
            return returnstring;
        }

        public override object Clone()
        {
            var n = new ListRemove(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            for (int i = 0; i<2; i++)
                if (string.IsNullOrEmpty(NonMacrolize(i)))
                    messages.Add(new ArgNotNullMessage(attributes[i].AttrCap, i, this));
            return messages;
        }
    }
}
