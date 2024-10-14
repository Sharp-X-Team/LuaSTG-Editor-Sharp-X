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
    [Serializable, NodeIcon("playerspeed.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class PlayerSpeed : TreeNode
    {
        [JsonConstructor]
        private PlayerSpeed() : base() { }

        public PlayerSpeed(DocumentData workSpaceData)
            : this(workSpaceData, "4, 2") { }

        public PlayerSpeed(DocumentData workSpaceData, string code)
            : base(workSpaceData)
        {
            Speed = code;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Speed
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "player.hspeed, player.lspeed = " + Macrolize(0) + "\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set this player's unfocused and focused speed respectively to \"" + attributes[0].AttrInput + "\"";
        }

        public override object Clone()
        {
            var n = new PlayerSpeed(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
