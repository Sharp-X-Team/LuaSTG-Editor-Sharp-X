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

namespace LuaSTGEditorSharp.EditorData.Node.Render
{
    [Serializable, NodeIcon("renderclear.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class RenderClear : TreeNode
    {
        [JsonConstructor]
        private RenderClear() : base() { }

        public RenderClear(DocumentData workSpaceData)
            : this(workSpaceData, "lstg.view3d.fog[3]") { }

        public RenderClear(DocumentData workSpaceData, string col)
            : base(workSpaceData)
        {
            Color = col;
        }

        [JsonIgnore, NodeAttribute]
        public string Color
        {
            get => DoubleCheckAttr(0, "ARGB", "Color").attrInput;
            set => DoubleCheckAttr(0, "ARGB", "Color").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"RenderClear({Macrolize(0)})\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Render clear with color ({NonMacrolize(0)})";
        }

        public override object Clone()
        {
            var n = new RenderClear(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
