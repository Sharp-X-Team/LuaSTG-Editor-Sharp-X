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

namespace LuaSTGEditorSharp.EditorData.Node.Audio
{
    [Serializable, NodeIcon("fadeoutbgm.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class FadeOutBGM : TreeNode
    {
        [JsonConstructor]
        public FadeOutBGM() : base() { }

        public FadeOutBGM(DocumentData workSpaceData)
            : this(workSpaceData, "\"\"", "64") { }

        public FadeOutBGM(DocumentData workSpaceData, string name, string duration)
            : base(workSpaceData)
        {
            Name = name;
            Duration = duration;
        }


        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0, "BGM").attrInput;
            set => DoubleCheckAttr(0, "BGM").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Duration
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            string s2 = Indent(2);
            yield return sp + "New(tasker, function()\n"
                        + sp + s1 + "local vol = 1\n"
                        + sp + s1 + $"for i = 0, {Macrolize(1)} do\n"
                        + sp + s2 + $"SetBGMVolume({Macrolize(0)}, vol)\n"
                        + sp + s2 + $"vol = vol - 1 / {Macrolize(1)}\n"
                        + sp + s2 + "task.Wait(1)\n"
                        + sp + s1 + "end\n"
                        + sp + s1 + $"StopMusic({Macrolize(0)})\n"
                        + sp + "end)\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Fade out music " + NonMacrolize(0) + " in " + NonMacrolize(1) + " frame(s)";
        }

        public override object Clone()
        {
            var n = new FadeOutBGM(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
