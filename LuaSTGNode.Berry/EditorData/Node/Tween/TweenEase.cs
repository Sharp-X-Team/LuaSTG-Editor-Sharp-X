using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Tween
{
    [Serializable, NodeIcon("tweenease.png")]
    [RequireParent(typeof(CreateTween))]
    [CreateInvoke(0), RCInvoke(0)]
    public class TweenEase : TreeNode
    {
        [JsonConstructor]
        private TweenEase() : base() { }

        public TweenEase(DocumentData workSpaceData)
            : this(workSpaceData, "linear")
        { }

        public TweenEase(DocumentData workSpaceData, string easing)
            : base(workSpaceData)
        {
            Easing = easing;
        }

        public string Easing
        {
            get => DoubleCheckAttr(0, "tween_easing").AttrInput;
            set => DoubleCheckAttr(0, "tween_easing").AttrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"last_tween:ease({Macrolize(0)})\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Ease tween with: Easing.{NonMacrolize(0)}";
        }

        public override object Clone()
        {
            var n = new TweenEase(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
