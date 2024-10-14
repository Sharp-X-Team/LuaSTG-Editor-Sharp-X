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
    [Serializable, NodeIcon("set3d.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class Set3D : TreeNode
    {
        [JsonConstructor]
        private Set3D() : base() { }

        public Set3D(DocumentData workSpaceData)
            : this(workSpaceData, "\"eye\"", "0, 0, 0") { }

        public Set3D(DocumentData workSpaceData, string td, string val)
            : base(workSpaceData)
        {
            Viewpoint = td;
            Value = val;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Viewpoint
        {
            get => DoubleCheckAttr(0, "viewpoint", "Viewpoint").attrInput;
            set => DoubleCheckAttr(0, "viewpoint", "Viewpoint").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Value
        {
            get => DoubleCheckAttr(1, name: "Value").attrInput;
            set => DoubleCheckAttr(1, name: "Value").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"Set3D({Macrolize(0)}, {Macrolize(1)})\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Set 3D viewpoint {NonMacrolize(0)} to value(s) ({NonMacrolize(1)})";
        }

        public override object Clone()
        {
            var n = new Set3D(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
