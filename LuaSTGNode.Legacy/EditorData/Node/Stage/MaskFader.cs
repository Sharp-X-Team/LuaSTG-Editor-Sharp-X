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

namespace LuaSTGEditorSharp.EditorData.Node.Stage
{
    [Serializable, NodeIcon("maskfader.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class MaskFader : TreeNode
    {
        [JsonConstructor]
        public MaskFader() : base() { }

        public MaskFader(DocumentData workSpaceData) : this(workSpaceData, "\"open\"") { }

        public MaskFader(DocumentData workSpaceData, string mode) : base(workSpaceData) 
        {
            Mode = mode;
        }

        [JsonIgnore, NodeAttribute]
        public string Mode
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "New(mask_fader, " + Macrolize(0) + ")\n";
        }

        public override string ToString()
        {
            return "Create mask fader with mode " + NonMacrolize(0) + "";
        }

        public override object Clone()
        {
            var n = new MaskFader(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}
