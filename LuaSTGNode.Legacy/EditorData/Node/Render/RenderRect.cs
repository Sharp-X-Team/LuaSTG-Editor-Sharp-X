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
    [Serializable, NodeIcon("renderrect.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class RenderRect : TreeNode
    {
        [JsonConstructor]
        private RenderRect() : base() { }

        public RenderRect(DocumentData workSpaceData)
            : this(workSpaceData, "\"img_void\"", "0", "0", "0", "0") { }

        public RenderRect(DocumentData workSpaceData, string img, string lef, string rig, string bot, string top)
            : base(workSpaceData)
        {
            Image = img;
            Left = lef;
            Right = rig;
            Bottom = bot;
            Top = top;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(0, "objimage", "Image").attrInput;
            set => DoubleCheckAttr(0, "objimage", "Image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Left
        {
            get => DoubleCheckAttr(1, name: "Left Point").attrInput;
            set => DoubleCheckAttr(1, name: "Left Point").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Right
        {
            get => DoubleCheckAttr(2, name: "Right Point").attrInput;
            set => DoubleCheckAttr(2, name: "Right Point").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Bottom
        {
            get => DoubleCheckAttr(3, name: "Bottom Point").attrInput;
            set => DoubleCheckAttr(3, name: "Bottom Point").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Top
        {
            get => DoubleCheckAttr(4, name: "Top Point").attrInput;
            set => DoubleCheckAttr(4, name: "Top Point").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "RenderRect(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + "," + Macrolize(3) + "," + Macrolize(4) + ")" + "\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Render Rect image " + NonMacrolize(0) + " at (" + NonMacrolize(1) + "," + NonMacrolize(2) + "," + NonMacrolize(3) + "," + NonMacrolize(4) + ")";
        }

        public override object Clone()
        {
            var n = new RenderRect(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
