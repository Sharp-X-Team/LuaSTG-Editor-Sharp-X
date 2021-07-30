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

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("properties.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class Property : TreeNode
    {
        [JsonConstructor]
        private Property() : base() { }

        public Property(DocumentData workSpaceData)
            : this(workSpaceData, "self", "", "") { }

        public Property(DocumentData workSpaceData, string unit, string proper, string valu)
            : base(workSpaceData)
        {
            Unity = unit;
            Ppty = proper;
            Vallu = valu;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Unity
        {
            get => DoubleCheckAttr(0, "target", "Unit").attrInput;
            set => DoubleCheckAttr(0, "target", "Unit").attrInput = value;
        }
        [JsonIgnore, NodeAttribute]
        public string Ppty
        {
            get => DoubleCheckAttr(1, name: "Property").attrInput;
            set => DoubleCheckAttr(1, name: "Property").attrInput = value;
        }
        [JsonIgnore, NodeAttribute]
        public string Vallu
        {
            get => DoubleCheckAttr(2, name: "Value").attrInput;
            set => DoubleCheckAttr(2, name: "Value").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "" + Macrolize(0) + "." + Macrolize(1) + " = " + Macrolize(2) + "\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set " + NonMacrolize(0) + "\'s " + NonMacrolize(1) + " to " + NonMacrolize(2) + "";
        }

        public override object Clone()
        {
            var n = new Property(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
