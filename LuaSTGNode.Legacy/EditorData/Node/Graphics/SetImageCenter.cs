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
    [Serializable, NodeIcon("setimagecenter.png")]
    [LeafNode]
    [RCInvoke(1)]
    public class SetImageCenter : TreeNode
    {
        [JsonConstructor]
        private SetImageCenter() : base() { }

        public SetImageCenter(DocumentData workSpaceData)
            : this(workSpaceData, "", "0, 0") { }

        public SetImageCenter(DocumentData workSpaceData, string tar, string pos)
            : base(workSpaceData)
        {
            Target = tar;
            Posit = pos;
            /*
            attributes.Add(new AttrItem("Target", tar, this, "target"));
            attributes.Add(new AttrItem("Blend Mode", blend, this, "blend"));
            attributes.Add(new AttrItem("ARGB", color, this, "ARGB"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "image", "Image").attrInput;
            set => DoubleCheckAttr(0, "image", "Image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Posit
        {
            get => DoubleCheckAttr(1, name: "X and Y").attrInput;
            set => DoubleCheckAttr(1, name: "X and Y").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "SetImageCenter(" + Macrolize(0) + ", " + Macrolize(1) + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set image center of " + NonMacrolize(0) + " to (" + NonMacrolize(1) + ")"; 
        }

        public override object Clone()
        {
            var n = new SetImageCenter(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
