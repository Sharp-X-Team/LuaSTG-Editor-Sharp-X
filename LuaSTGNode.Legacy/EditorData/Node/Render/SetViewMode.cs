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
    [Serializable, NodeIcon("setviewmode.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class SetViewMode : TreeNode
    {
        [JsonConstructor]
        private SetViewMode() : base() { }

        public SetViewMode(DocumentData workSpaceData)
            : this(workSpaceData, "world") { }

        public SetViewMode(DocumentData workSpaceData, string mod)
            : base(workSpaceData)
        {
            Mode = mod;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Mode
        {
            get => DoubleCheckAttr(0, "viewmode", "View Mode").attrInput;
            set => DoubleCheckAttr(0, "viewmode", "View Mode").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "SetViewMode\'" + Macrolize(0) + "\'\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set view mode to \"" + attributes[0].AttrInput + "\"";
        }

        public override object Clone()
        {
            var n = new SetViewMode(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
