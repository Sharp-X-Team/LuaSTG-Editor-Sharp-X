using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Message;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("playeroption.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(3)]
    public class PlayerOption : TreeNode
    {
        [JsonConstructor]
        private PlayerOption() : base() { }

        public PlayerOption(DocumentData workSpaceData)
            : this(workSpaceData, "", "", "0.5,0.5") { }

        public PlayerOption(DocumentData workSpaceData, string img, string resn, string collisize) : base(workSpaceData)
        {
            Image = img;
            Resname = resn;
            CollisionSize = collisize;
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(0, "objimage").attrInput;
            set => DoubleCheckAttr(0, "objimage").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Resname
        {
            get => DoubleCheckAttr(1, name: "Resource name").attrInput;
            set => DoubleCheckAttr(1, name: "Resource name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string CollisionSize
        {
            get => DoubleCheckAttr(2, "size", "Hitbox size").attrInput;
            set => DoubleCheckAttr(2, "size", "Hitbox size").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "LoadTexture('" + Macrolize(1) + "\n";
        }

        public override string ToString()
        {
            return "Set walk image of current player from \"" + NonMacrolize(0) + "\" and its behaviour to that of player";
        }

        public override object Clone()
        {
            var n = new PlayerOption(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(2, this);
        }
    }
}
