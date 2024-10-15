using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Audio
{
    [Serializable, NodeIcon("bgmvolume.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class SetBGMVolume : TreeNode
    {
        [JsonConstructor]
        private SetBGMVolume() : base() { }

        public SetBGMVolume(DocumentData workSpaceData)
            : this(workSpaceData, "1", "") { }

        public SetBGMVolume(DocumentData workSpaceData, string vol, string name)
            : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Name", name, this, "BGM"));
            attributes.Add(new AttrItem("Time", time, this));
            attributes.Add(new AttrItem("Set stage time", setstime, this, "bool"));
            */
            Volume = vol;
            Name = name;
        }

        [JsonIgnore, NodeAttribute]
        public string Volume
        {
            get => DoubleCheckAttr(0, name: "Volume (0-1)").attrInput;
            set => DoubleCheckAttr(0, name: "Volume (0-1)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(1, "BGM").attrInput;
            set => DoubleCheckAttr(1, "BGM").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            if (!string.IsNullOrEmpty(Macrolize(1))) {
                yield return sp + "SetBGMVolume(" + Macrolize(1) + ", " + Macrolize(0) + ")\n";
            } 
            else
            {
                yield return sp + "SetBGMVolume(" + Macrolize(0) + ")\n";
            }
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(NonMacrolize(1)))
            {
                return "Set BGM " + NonMacrolize(1) + "'s volume to " + NonMacrolize(0);
            }
            else
            {
                return "Set BGM volume to " + NonMacrolize(0);
            }
        }

        public override object Clone()
        {
            var n = new SetBGMVolume(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
