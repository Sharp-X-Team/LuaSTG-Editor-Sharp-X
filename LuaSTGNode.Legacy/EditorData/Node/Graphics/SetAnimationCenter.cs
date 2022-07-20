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
    [Serializable, NodeIcon("setanimationcenter.png")]
    [LeafNode]
    [RCInvoke(1)]
    public class SetAnimationCenter : TreeNode
    {
        [JsonConstructor]
        private SetAnimationCenter() : base() { }

        public SetAnimationCenter(DocumentData workSpaceData)
            : this(workSpaceData, "", "0,0") { }

        public SetAnimationCenter(DocumentData workSpaceData, string tar, string pos)
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
            get => DoubleCheckAttr(0, "objimage", "Image (select only anim)").attrInput;
            set => DoubleCheckAttr(0, "objimage", "Image (select only anim)").attrInput = value;
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
            yield return sp + "SetAnimationCenter(" + Macrolize(0) + "," + Macrolize(1) + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set animation center of " + NonMacrolize(0) + " to (" + NonMacrolize(1) + ")"; 
        }

        public override object Clone()
        {
            var n = new SetAnimationCenter(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
