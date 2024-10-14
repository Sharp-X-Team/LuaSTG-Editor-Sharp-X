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
    [Serializable, NodeIcon("rendertext.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class RenderText : TreeNode
    {
        [JsonConstructor]
        private RenderText() : base() { }

        public RenderText(DocumentData workSpaceData)
            : this(workSpaceData, "", "\"\"", "self.x, self.y", "1", "0") { }

        public RenderText(DocumentData workSpaceData, string font, string strn, string posit, string sizez, string aline)
            : base(workSpaceData)
        {
            Fontt = font;
            Stringz = strn;
            Pos = posit;
            Size = sizez;
            Alinn = aline;
        }

        [JsonIgnore, NodeAttribute]
        public string Fontt
        {
            get => DoubleCheckAttr(0, "font", "Font name").attrInput;
            set => DoubleCheckAttr(0, "font", "Font name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Stringz
        {
            get => DoubleCheckAttr(1, "multilineText", "Text").attrInput;
            set => DoubleCheckAttr(1, "multilineText", "Text").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Pos
        {
            get => DoubleCheckAttr(2, "position", "Position").attrInput;
            set => DoubleCheckAttr(2, "position", "Position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Size
        {
            get => DoubleCheckAttr(3, name: "Scale").attrInput;
            set => DoubleCheckAttr(3, name: "Scale").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Alinn
        {
            get => DoubleCheckAttr(4, "alignInput", "Align").attrInput;
            set => DoubleCheckAttr(4, "alignInput", "Align").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "lstg.RenderText(" + Macrolize(0) + ", " + Macrolize(1) + ", " + Macrolize(2) + ", " + Macrolize(3) + ", " + Macrolize(4) + ")\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Render text " + NonMacrolize(1) + " with font " + NonMacrolize(0) + "\n" +
                "Position = (" + NonMacrolize(2) + ") Scale = (" + NonMacrolize(3) + ") Align = (" + NonMacrolize(4) + ")";
        }

        public override object Clone()
        {
            var n = new RenderText(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
