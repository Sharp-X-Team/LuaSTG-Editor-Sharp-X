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
    [Serializable, NodeIcon("gdatabombpiece.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class SetGameBombPiece : TreeNode
    {
        [JsonConstructor]
        private SetGameBombPiece() : base() { }

        public SetGameBombPiece(DocumentData workSpaceData)
            : this(workSpaceData, "0") { }

        public SetGameBombPiece(DocumentData workSpaceData, string value)
            : base(workSpaceData)
        {
            Value = value;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Value
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "lstg.var.bomb_chip = " + Macrolize(0) + "\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set player bomb pieces to " + NonMacrolize(0) + "";
        }

        public override object Clone()
        {
            var n = new SetGameBombPiece(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
