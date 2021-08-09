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
    [Serializable, NodeIcon("raisewarning.png")]
    [LeafNode]
    [RCInvoke(0)]
    public class RaiseWarning : TreeNode
    {
        [JsonConstructor]
        private RaiseWarning() : base() { }

        public RaiseWarning(DocumentData workSpaceData)
            : this(workSpaceData, "\"\"") { }

        public RaiseWarning(DocumentData workSpaceData, string code)
            : base(workSpaceData)
        {
            Text = code;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Text
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "lstg.MsgBoxWarn(" + Macrolize(0) + ")\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Raise warning named " + attributes[0].AttrInput + "";
        }

        public override object Clone()
        {
            var n = new RaiseWarning(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
