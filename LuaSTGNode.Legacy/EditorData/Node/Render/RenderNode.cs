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
    [Serializable, NodeIcon("rendernode.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class RenderNode : TreeNode
    {
        [JsonConstructor]
        private RenderNode() : base() { }

        public RenderNode(DocumentData workSpaceData)
            : this(workSpaceData, "\"img_void\"", "0,0", "0", "1,1", "0.5") { }

        public RenderNode(DocumentData workSpaceData, string img, string pos, string ang, string siz, string zed)
            : base(workSpaceData)
        {
            Image = img;
            Place = pos;
            Angle = ang;
            Size = siz;
            Depth = zed;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(0, "objimage", "Image").attrInput;
            set => DoubleCheckAttr(0, "objimage", "Image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Place
        {
            get => DoubleCheckAttr(1, "position", "Position").attrInput;
            set => DoubleCheckAttr(1, "position", "Position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Angle
        {
            get => DoubleCheckAttr(2, name: "Angle").attrInput;
            set => DoubleCheckAttr(2, name: "Angle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Size
        {
            get => DoubleCheckAttr(3, "size", "Scale").attrInput;
            set => DoubleCheckAttr(3, "size", "Scale").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Depth
        {
            get => DoubleCheckAttr(4, name: "Z").attrInput;
            set => DoubleCheckAttr(4, name: "Z").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "Render(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + "," + Macrolize(3) + "," + Macrolize(4) + ")" + "\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Render image " + NonMacrolize(0) + " at (" + NonMacrolize(1) + "), angle is \"" + NonMacrolize(2) + "\" with size (" + NonMacrolize(3) + ") (Z = " + NonMacrolize(4) + ")";
        }

        public override object Clone()
        {
            var n = new RenderNode(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
