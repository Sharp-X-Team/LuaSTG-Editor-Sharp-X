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
    [Serializable, NodeIcon("objectlayer.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class SetLayer : TreeNode
    {
        [JsonConstructor]
        private SetLayer() : base() { }

        public SetLayer(DocumentData workSpaceData)
            : this(workSpaceData, "self", "LAYER_ENEMY_BULLET") { }

        public SetLayer(DocumentData workSpaceData, string unit, string valu)
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
            get => DoubleCheckAttr(1, "layer", "Value").attrInput;
            set => DoubleCheckAttr(1, "layer", "Value").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"{Macrolize(0)}.layer = {Macrolize(1)}\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Set {NonMacrolize(0)}\'s layer to {NonMacrolize(1)}";
        }

        public override object Clone()
        {
            var n = new SetLayer(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
