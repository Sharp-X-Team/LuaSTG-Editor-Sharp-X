using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("unitpreserve.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class UnitPreserve : TreeNode
    {
        [JsonConstructor]
        private UnitPreserve() : base() { }

        public UnitPreserve(DocumentData workSpaceData)
            : this(workSpaceData, "self") { }

        public UnitPreserve(DocumentData workSpaceData, string code)
            : base(workSpaceData)
        {
            Unit = code;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Unit
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "PreserveObject(" + Macrolize(0) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Preserve " + attributes[0].AttrInput + "";
        }

        public override object Clone()
        {
            var n = new UnitPreserve(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
