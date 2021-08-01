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
    [Serializable, NodeIcon("renderttf.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class RenderTTF : TreeNode
    {
        [JsonConstructor]
        private RenderTTF() : base() { }

        public RenderTTF(DocumentData workSpaceData)
            : this(workSpaceData, "", "", "0", "0", "0", "0", "0", "255, 255, 255, 255", "1") { }

        public RenderTTF(DocumentData workSpaceData, string font, string strn, string left, string right, string bottom, string top, string formt, string color, string scale)
            : base(workSpaceData)
        {
            Fontt = font;
            Stringz = strn;
            LFT = left;
            RGT = right;
            BOT = bottom;
            TOP = top;
            Fmat = formt;
            Col = color;
            Size = scale;
        }

        [JsonIgnore, NodeAttribute]
        public string Fontt
        {
            get => DoubleCheckAttr(0, "ttf", "TTF name").attrInput;
            set => DoubleCheckAttr(0, "ttf", "TTF name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Stringz
        {
            get => DoubleCheckAttr(1, "multilineText", "Text").attrInput;
            set => DoubleCheckAttr(1, "multilineText", "Text").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string LFT
        {
            get => DoubleCheckAttr(2, name: "Left").attrInput;
            set => DoubleCheckAttr(2, name: "Left").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string RGT
        {
            get => DoubleCheckAttr(3, name: "Right").attrInput;
            set => DoubleCheckAttr(3, name: "Right").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string BOT
        {
            get => DoubleCheckAttr(4, name: "Bottom").attrInput;
            set => DoubleCheckAttr(4, name: "Bottom").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string TOP
        {
            get => DoubleCheckAttr(5, name: "Top").attrInput;
            set => DoubleCheckAttr(5, name: "Top").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Fmat
        {
            get => DoubleCheckAttr(6, name: "Format").attrInput;
            set => DoubleCheckAttr(6, name: "Format").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Col
        {
            get => DoubleCheckAttr(7, "ARGB", "Color").attrInput;
            set => DoubleCheckAttr(7, "ARGB", "Color").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Size
        {
            get => DoubleCheckAttr(8, name: "Scale").attrInput;
            set => DoubleCheckAttr(8, name: "Scale").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "lstg.RenderTTF(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + "," + Macrolize(3) + "," + Macrolize(4) + "," + Macrolize(5) + "," + Macrolize(6) + ",Color(" + Macrolize(7) + ")," + Macrolize(8) + ")\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Render TTF " + NonMacrolize(1) + " with font \"" + NonMacrolize(0) + "\"\n" +
                "Position = (" + NonMacrolize(2) + "," + NonMacrolize(3) + "," + NonMacrolize(4) + "," + NonMacrolize(5) + ")" +
                " Format = (" + NonMacrolize(6) + ") Color = (" + NonMacrolize(7) + "), Scale = (" + NonMacrolize(8) + ")";
        }

        public override object Clone()
        {
            var n = new RenderTTF(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
