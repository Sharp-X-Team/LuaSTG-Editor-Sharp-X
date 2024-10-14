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
    [Serializable, NodeIcon("objectsize.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class SetSize : TreeNode
    {
        [JsonConstructor]
        private SetSize() : base() { }

        public SetSize(DocumentData workSpaceData)
            : this(workSpaceData, "self", "1, 1") { }

        public SetSize(DocumentData workSpaceData, string unit, string valu)
            : base(workSpaceData)
        {
            Unity = unit;
            Ppty = valu;
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
            get => DoubleCheckAttr(1, "size", "Scale").attrInput;
            set => DoubleCheckAttr(1, "size", "Scale").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"{Macrolize(0)}.hscale, {Macrolize(0)}.vscale = {Macrolize(1)}\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set " + NonMacrolize(0) + "\'s scale to (" + NonMacrolize(1) + ")";
        }

        public override object Clone()
        {
            var n = new SetSize(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
