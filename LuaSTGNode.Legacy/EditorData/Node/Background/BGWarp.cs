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

namespace LuaSTGEditorSharp.EditorData.Node.Render
{
    [Serializable, NodeIcon("bgstagewarp.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [RCInvoke(1)]
    public class BGWarp : TreeNode
    {
        [JsonConstructor]
        public BGWarp() : base() { }

        public BGWarp(DocumentData workSpaceData) : this(workSpaceData, "Capture") { }

        public BGWarp(DocumentData workSpaceData, string operation) : base(workSpaceData) 
        {
            Operation = operation;
        }

        [JsonIgnore, NodeAttribute("Capture")]
        public string Operation
        {
            get => DoubleCheckAttr(0, "warptarget").attrInput;
            set => DoubleCheckAttr(0, "warptarget").attrInput = value;
        }

        public override string ToString()
        {
            return "Background warp effect (\"" + NonMacrolize(0) + "\")";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "background.WarpEffect" + Macrolize(0) + "()\n";
        }

        public override object Clone()
        {
            var n = new BGWarp(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}
