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
    [Serializable, NodeIcon("gdatakillplayer.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class SetGameKillPlayer : TreeNode
    {
        [JsonConstructor]
        private SetGameKillPlayer() : base() { }

        public SetGameKillPlayer(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            //Value = value;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "player.death = 100\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Kill player";
        }

        public override object Clone()
        {
            var n = new SetGameKillPlayer(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
