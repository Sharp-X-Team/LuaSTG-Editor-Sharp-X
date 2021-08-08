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

namespace LuaSTGEditorSharp.EditorData.Node.Render
{
    [Serializable, NodeIcon("camerasetter.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class CameraSetter : TreeNode
    {
        [JsonConstructor]
        private CameraSetter() : base() { }

        public CameraSetter(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "last=New(camera_setter)\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Create camera setter";
        }

        public override object Clone()
        {
            var n = new CameraSetter(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
