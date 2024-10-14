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
    [Serializable, NodeIcon("playeroptionpos.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class PlayerOptionPosition : TreeNode
    {
        [JsonConstructor]
        private PlayerOptionPosition() : base() { }

        public PlayerOptionPosition(DocumentData workSpaceData)
            : this(workSpaceData, "0, 0",  "0, 0") { }

        public PlayerOptionPosition(DocumentData workSpaceData, string unfpos, string focpos)
            : base(workSpaceData)
        {
            UnfocusedPos = unfpos;
            FocusedPos = focpos;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string UnfocusedPos
        {
            get => DoubleCheckAttr(0, "position", "Unfocused Position").attrInput;
            set => DoubleCheckAttr(0, "position", "Unfocused Position").attrInput = value;
        }
        [JsonIgnore, NodeAttribute]
        public string FocusedPos
        {
            get => DoubleCheckAttr(1, "position", "Focused Position").attrInput;
            set => DoubleCheckAttr(1, "position", "Focused Position").attrInput = value;
        }

        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Option at (unfocused: \"" + NonMacrolize(0) + "\"), (focused: \"" + NonMacrolize(1) + "\")";
        }

        public override object Clone()
        {
            var n = new PlayerOptionPosition(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
