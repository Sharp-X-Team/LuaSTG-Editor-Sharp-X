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
    [Serializable, NodeIcon("hinter.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class Hinter : TreeNode
    {
        [JsonConstructor]
        public Hinter() : base() { }

        public Hinter(DocumentData workSpaceData)
            : this(workSpaceData, "\"leaf\"", "1", "self.x, self.y", "60", "60", "false") { }

        public Hinter(DocumentData workSpaceData, string img, string size, string position, string t1, string t2, string fade)
            : base(workSpaceData)
        {
            Image = img;
            Size = size;
            Position = position;
            T1 = t1;
            T2 = t2;
            Fade = fade;
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(0, "image").attrInput;
            set => DoubleCheckAttr(0, "image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Size
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(2, "position").attrInput;
            set => DoubleCheckAttr(2, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string T1
        {
            get => DoubleCheckAttr(3, name:"Fade in/out time").attrInput;
            set => DoubleCheckAttr(3, name:"Fade in/out time").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string T2
        {
            get => DoubleCheckAttr(4, name: "Keep Time").attrInput;
            set => DoubleCheckAttr(4, name: "Keep Time").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Fade
        {
            get => DoubleCheckAttr(5, "bool", "Opacity Fading").attrInput;
            set => DoubleCheckAttr(5, "bool", "Opacity Fading").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"New(hinter, {Macrolize(0)}, {Macrolize(1)}, {Macrolize(2)}, {Macrolize(3)}, {Macrolize(4)}, {Macrolize(5)})\n";
        }

        public override string ToString()
        {
            return "Create hinter at (" + NonMacrolize(2) + ") of size " + NonMacrolize(1) + " lasting " + NonMacrolize(4) + " frame(s)";
        }

        public override object Clone()
        {
            // I nearly misstyped "Hinter" with something far worse not really correct.
            var n = new Hinter(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}
