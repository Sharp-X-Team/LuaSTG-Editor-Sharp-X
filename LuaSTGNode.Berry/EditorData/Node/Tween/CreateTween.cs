using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Tween
{
    [Serializable, NodeIcon("createtween.png")]
    [CreateInvoke(0), RCInvoke(0)]
    public class CreateTween : TreeNode
    {
        [JsonConstructor]
        private CreateTween() : base() { }

        public CreateTween(DocumentData workSpaceData)
            : this(workSpaceData, "self", "{}", "60")
        { }

        public CreateTween(DocumentData workSpaceData, string target, string props, string duration)
            : base(workSpaceData)
        {
            Target = target;
            Properties = props;
            Duration = duration;
        }

        public string Target
        {
            get => DoubleCheckAttr(0, "unit").AttrInput;
            set => DoubleCheckAttr(0, "unit").AttrInput = value;
        }

        public string Properties
        {
            get => DoubleCheckAttr(1, "", "Properties").AttrInput;
            set => DoubleCheckAttr(1, "", "Properties").AttrInput = value;
        }

        public string Duration
        {
            get => DoubleCheckAttr(2, "", "Duration (frames)").AttrInput;
            set => DoubleCheckAttr(2, "", "Duration (frames)").AttrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"last_tween = Tween.to({Macrolize(0)}, {Macrolize(1)}, {Macrolize(2)})\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
            }
            yield return sp + "TweenManager:add(last_tween)\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Create a tween for \"{NonMacrolize(0)}\" with ({NonMacrolize(1)}) for {NonMacrolize(2)} frame(s)";
        }

        public override object Clone()
        {
            var n = new CreateTween(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
