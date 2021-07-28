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
    [Serializable, NodeIcon("setpace.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class SetPace : TreeNode
    {
        [JsonConstructor]
        private SetPace() : base() { }

        public SetPace(DocumentData workSpaceData)
            : this(workSpaceData, "0", "0") { }

        public SetPace(DocumentData workSpaceData, string stime, string ptime)
            : base(workSpaceData)
        {
            StartTime = stime;
            PaceTime = ptime;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string StartTime
        {
            get => DoubleCheckAttr(0, name: "Start Time").attrInput;
            set => DoubleCheckAttr(0, name: "Start Time").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string PaceTime
        {
            get => DoubleCheckAttr(1, name: "Pace Time").attrInput;
            set => DoubleCheckAttr(1, name: "Pace Time").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "ex.meterstart = " + Macrolize(0) + "; ex.meterclock = " + Macrolize(1) + "\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set pace start " + NonMacrolize(0) + " seconds, tick " + NonMacrolize(1) + " seconds";
        }

        public override object Clone()
        {
            var n = new TaskWait(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
