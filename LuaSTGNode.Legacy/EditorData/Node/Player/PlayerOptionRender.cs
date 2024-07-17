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
    [Serializable, NodeIcon("playeroptionrender.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class PlayerOptionRender : TreeNode
    {
        [JsonConstructor]
        private PlayerOptionRender() : base() { }

        public PlayerOptionRender(DocumentData workSpaceData)
            : this(workSpaceData, "\"leaf\"", "self.timer * 3", "1,1") { }

        public PlayerOptionRender(DocumentData workSpaceData, string img, string ang, string size)
            : base(workSpaceData)
        {
            Image = img;
            AngleVal = ang;
            Size = size;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(0, "objimage").attrInput;
            set => DoubleCheckAttr(0, "objimage").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string AngleVal
        {
            get => DoubleCheckAttr(1, name: "Angle").attrInput;
            set => DoubleCheckAttr(1, name: "Angle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Size
        {
            get => DoubleCheckAttr(2, "size", "Scale").attrInput;
            set => DoubleCheckAttr(2, "size", "Scale").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            string help = "" +
                sp + "for i = 1, 4 do\n" +
                sp + s1 + "if self.sp[i] and self.sp[i][3] > 0.5 then\n" +
                sp + s1 + s1 + $"Render(" + Macrolize(0) + ", self.supportx + self.sp[i][1], self.supporty + self.sp[i][2], " + Macrolize(1) + ", " + Macrolize(2) + ")\n" +
                sp + s1 + "end\n" +
                sp + "end\n";
            yield return sp + help;
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Render player options with image " + NonMacrolize(0) + " at angle \"" + NonMacrolize(1) + "\" with size (" + NonMacrolize(2) + ")";
        }

        public override object Clone()
        {
            var n = new PlayerOptionRender(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
