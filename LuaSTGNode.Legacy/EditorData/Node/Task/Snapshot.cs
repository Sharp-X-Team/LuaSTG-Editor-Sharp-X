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
    [Serializable, NodeIcon("snapshot.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class Snapshot : TreeNode
    {
        [JsonConstructor]
        private Snapshot() : base() { }

        public Snapshot(DocumentData workSpaceData)
            : this(workSpaceData, "os.date(\"!%Y-%m-%d-%H-%M-%S\", os.time() + setting.timezone * 3600)") { }

        public Snapshot(DocumentData workSpaceData, string name)
            : base(workSpaceData)
        {
            Name = name;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0, name: "File name").attrInput;
            set => DoubleCheckAttr(0, name: "File name").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string snappath = "\"snapshot" + "\\\\" + "\"";
            yield return sp + "Snapshot(" + snappath + " .. " + Macrolize(0) + " .. \".png\")\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Snapshot screen and save as \"" + attributes[0].AttrInput + "\"";
        }

        public override object Clone()
        {
            var n = new Snapshot(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
