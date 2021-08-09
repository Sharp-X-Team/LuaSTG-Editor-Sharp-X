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
    [Serializable, NodeIcon("playertarget.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class PlayerFindTarget : TreeNode
    {
        [JsonConstructor]
        private PlayerFindTarget() : base() { }

        public PlayerFindTarget(DocumentData workSpaceData)
            : this(workSpaceData, "self") { }

        public PlayerFindTarget(DocumentData workSpaceData, string code)
            : base(workSpaceData)
        {
            Target = code;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "player_class.findtarget(" + Macrolize(0) + ")\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Find player target to \"" + attributes[0].AttrInput + "\"";
        }

        public override object Clone()
        {
            var n = new PlayerFindTarget(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
