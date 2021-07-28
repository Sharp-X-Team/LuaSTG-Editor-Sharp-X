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
    [Serializable, NodeIcon("setfps.png")]
    [LeafNode]
    [RCInvoke(0)]
    public class SetFPS : TreeNode
    {
        [JsonConstructor]
        private SetFPS() : base() { }

        public SetFPS(DocumentData workSpaceData)
            : this(workSpaceData, "60") { }

        public SetFPS(DocumentData workSpaceData, string code)
            : base(workSpaceData)
        {
            Frames = code;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Frames
        {
            get => DoubleCheckAttr(0, "fpsset").attrInput;
            set => DoubleCheckAttr(0, "fpsset").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "SetFPS(" + Macrolize(0) + ")\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set framerate to " + attributes[0].AttrInput + " frame(s)";
        }

        public override object Clone()
        {
            var n = new SetFPS(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
