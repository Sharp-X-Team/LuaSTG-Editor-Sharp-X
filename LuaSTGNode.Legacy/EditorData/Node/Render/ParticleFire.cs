using System;
using System.Collections.Generic;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Render
{
    [Serializable, NodeIcon("particlefire.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class ParticleFire : TreeNode
    {
        [JsonConstructor]
        private ParticleFire() : base() { }

        public ParticleFire(DocumentData workSpaceData)
            : this(workSpaceData, "self") { }

        public ParticleFire(DocumentData workSpaceData, string unit)
            : base(workSpaceData)
        {
            Unit = unit;
        }

        [JsonIgnore, NodeAttribute]
        public string Unit
        {
            get => DoubleCheckAttr(0, "target", "Unit").attrInput;
            set => DoubleCheckAttr(0, "target", "Unit").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "ParticleFire(" + Macrolize(0) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Start firing particles of " + NonMacrolize(0);
        }

        public override object Clone()
        {
            var n = new ParticleFire(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
