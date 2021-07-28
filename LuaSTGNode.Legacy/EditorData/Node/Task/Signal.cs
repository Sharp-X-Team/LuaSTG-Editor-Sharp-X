using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Task
{

    [Serializable, NodeIcon("tasker.png")]
    //[RequireAncestor(typeof(CodeAlikeTypes))]
    public class Signal : TreeNode
    {
        [JsonConstructor]
        private Signal() : base() { }

        public Signal(DocumentData workSpaceData)
            : this(workSpaceData, "Name", "Value")
        { }

        public Signal(DocumentData workSpaceData, string signalname, string signalvalue)
            : base(workSpaceData)
        {
            SignalName = signalname;
            SignalValue = signalvalue;
        }

        [JsonIgnore, NodeAttribute]
        public string SignalName
        {
            get => DoubleCheckAttr(0, name: "Signal Name").attrInput;
            set => DoubleCheckAttr(0, name: "Signal Name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string SignalValue
        {
            get => DoubleCheckAttr(1, name: "Signal Value").attrInput;
            set => DoubleCheckAttr(1, name: "Signal Value").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "ex.SetSignal(" + Macrolize(0) + "," + Macrolize(1) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set signal " + NonMacrolize(0) + ", with value: " + NonMacrolize(1);
        }

        public override object Clone()
        {
            var n = new Tasker(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
