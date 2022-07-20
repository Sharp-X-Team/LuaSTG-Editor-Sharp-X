using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Stage
{
    [Serializable, NodeIcon("stagefinishreplay.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class StageFinishReplay : TreeNode
    {
        [JsonConstructor]
        public StageFinishReplay() : base() { }

        public StageFinishReplay(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "stage.group.FinishReplay()\n";
        }

        public override string ToString()
        {
            return "Finish stage replay";
        }

        public override object Clone()
        {
            var n = new StageFinishReplay(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}
