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
    //[CreateInvoke(0), RCInvoke(3)]
    public class PlayerOptionPower : TreeNode
    {
        [JsonConstructor]
        private PlayerOptionPower() : base() { }

        public PlayerOptionPower(DocumentData workSpaceData)
            : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string[] pos = { "nil", "nil", "nil", "nil" };
            int i = 0;
            foreach(TreeNode node in Children)
            {
                pos[i] = "{" + node.NonMacrolize(0) + ", " + node.NonMacrolize(1) + " }";
                i++;
            }
            
            yield return sp + "{" + pos[0] + "," + pos[1] + "," + pos[2] + "," + pos[3] + "},\n";
        }

        public override string ToString()
        {
            return $"Set options in power level {Children.Count}";
        }

        public override object Clone()
        {
            var n = new PlayerOptionPower(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(2, this);
        }
    }
}
