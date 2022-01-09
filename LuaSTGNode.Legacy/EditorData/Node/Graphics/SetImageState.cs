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

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("setimagestate.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(2)]
    public class SetImageState : TreeNode
    {
        [JsonConstructor]
        private SetImageState() : base() { }

        public SetImageState(DocumentData workSpaceData)
            : this(workSpaceData, "", "\"\"", "255,255,255,255") { }

        public SetImageState(DocumentData workSpaceData, string tar, string blend, string color)
            : base(workSpaceData)
        {
            Target = tar;
            BlendMode = blend;
            ARGB = color;
            /*
            attributes.Add(new AttrItem("Target", tar, this, "target"));
            attributes.Add(new AttrItem("Blend Mode", blend, this, "blend"));
            attributes.Add(new AttrItem("ARGB", color, this, "ARGB"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "image", "Image name").attrInput;
            set => DoubleCheckAttr(0, "image", "Image name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string BlendMode
        {
            get => DoubleCheckAttr(1, "blend", "Blend Mode").attrInput;
            set => DoubleCheckAttr(1, "blend", "Blend Mode").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ARGB
        {
            get => DoubleCheckAttr(2, "ARGB").attrInput;
            set => DoubleCheckAttr(2, "ARGB").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "SetImageState(" + Macrolize(0) + "," + Macrolize(1) + ",Color(" + Macrolize(2) + "))\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set image state of image " + NonMacrolize(0) + " to (" + NonMacrolize(2) + "), blend mode to " 
                + NonMacrolize(1);
        }

        public override object Clone()
        {
            var n = new SetImageState(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
