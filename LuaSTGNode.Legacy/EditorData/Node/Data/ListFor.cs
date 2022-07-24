using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("listfor.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [RCInvoke(0)]
    public class ListFor : TreeNode
    {
        [JsonConstructor]
        public ListFor() : base() { }

        public ListFor(DocumentData workSpaceData)
            : this(workSpaceData, "last_list", "true") { }

        public ListFor(DocumentData workSpaceData, string list, string method)
            : base(workSpaceData)
        {
            List = list;
            Method = method;
        }

        [JsonIgnore, NodeAttribute]
        public string List
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Method
        {
            get => DoubleCheckAttr(1, "bool", name: "Ipairs").attrInput;
            set => DoubleCheckAttr(1, "bool", name: "Ipairs").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string met = NonMacrolize(1) == "true" ? "ipairs" : "pairs";
            yield return sp + "for key, value in " + met + "(" + Macrolize(0) + ") do\n";
            foreach (var i in base.ToLua(spacing + 1))
            {
                yield return i;
            }
            yield return sp + "end\n";
        }

        public override string ToString()
        {
            return "For each items (key, value) in list " + NonMacrolize(0);
        }

        public override object Clone()
        {
            var n = new ListFor(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (var i in GetChildLines())
            {
                yield return i;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}
