using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("setsplash.png")]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    class SetSplash : TreeNode
    {
        [JsonConstructor]
        private SetSplash() : base() { }

        public SetSplash(DocumentData workSpaceData)
            : this(workSpaceData, "true") { }

        public SetSplash(DocumentData workSpaceData, string show)
            : base(workSpaceData)
        {
            ShowMouse = show;
        }

        [JsonIgnore, NodeAttribute]
        public string ShowMouse
        {
            get => DoubleCheckAttr(0, "bool", "Display mouse on screen").attrInput;
            set => DoubleCheckAttr(0, "bool", "Display mouse on screen").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"SetSplash({Macrolize(0)})\n";
        }

        public override string ToString()
        {
            if (NonMacrolize(0) == "true")
                return "Show the mouse on game screen";
            else
                return "Don't show the mouse on game screen";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override object Clone()
        {
            var n = new SetSplash(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}