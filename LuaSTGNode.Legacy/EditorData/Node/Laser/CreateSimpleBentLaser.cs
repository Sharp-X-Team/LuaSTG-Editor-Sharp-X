using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Laser
{
    [Serializable, NodeIcon("laserbentsimple.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [RCInvoke(0)]
    class CreateSimpleBentLaser : TreeNode
    {
        [JsonConstructor]
        public CreateSimpleBentLaser() : base() { }

        public CreateSimpleBentLaser(DocumentData workSpaceData)
            : this(workSpaceData, "self.x, self.y", "COLOR_RED", "32", "8", "false", "0") { }

        public CreateSimpleBentLaser(DocumentData workSpaceData, string position, string color, string length, string width
            , string sampling, string node)
            : base(workSpaceData)
        {
            Position = position;
            Color = color;
            LengthInFrames = length;
            Width = width;
            Sampling = sampling;
            Node = node;
        }

        public override object Clone()
        {
            CreateSimpleBentLaser n = new(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            bool hasChildren = Children.Count > 0;
            return $"Create simple bent laser{(hasChildren ? " with task" : "")} at ({NonMacrolize(0)})";
        }

        #region Attr

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(0, "position").attrInput;
            set => DoubleCheckAttr(0, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Color
        {
            get => DoubleCheckAttr(1, "color").attrInput;
            set => DoubleCheckAttr(1, "color").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string LengthInFrames
        {
            get => DoubleCheckAttr(2, "time", "Length (in frames)").attrInput;
            set => DoubleCheckAttr(2, "time", "Length (in frames)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Width
        {
            get => DoubleCheckAttr(3, "length").attrInput;
            set => DoubleCheckAttr(3, "length").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Sampling
        {
            get => DoubleCheckAttr(4, "bool", "Thunder").attrInput;
            set => DoubleCheckAttr(4, "bool", "Thunder").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Node
        {
            get => DoubleCheckAttr(5, "length").attrInput;
            set => DoubleCheckAttr(5, "length").attrInput = value;
        }

        #endregion

        public override IEnumerable<string> ToLua(int spacing)
        {
            bool hasChildren = Children.Count > 0;
            string sp = Indent(spacing);
            string sp1 = Indent(spacing + 1);
            string sampling = NonMacrolize(4);
            if (sampling == "true")
            {
                sampling = "0";
            }
            if (sampling == "false")
            {
                sampling = "4";
            }
            yield return sp + $"last = New(laser_bent, {Macrolize(1)}, {Macrolize(0)}, {Macrolize(2)}, " +
                $"{Macrolize(3)}, {sampling}, {Macrolize(5)})\n";
            yield return sp + $"laser_bent.init(last, {Macrolize(1)}, {Macrolize(0)}, {Macrolize(2)}, {Macrolize(3)}, {sampling}, {Macrolize(5)})\n";
            if (hasChildren)
            {
                yield return sp + $"lasttask = task.New(last, function()\n";
                yield return sp1 + "local self = task.GetSelf()\n";
                foreach (var a in base.ToLua(spacing + 1))
                {
                    yield return a;
                }
                yield return sp + "end)\n";
            }
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            bool hasChildren = Children.Count > 0;
            if (hasChildren)
            {
                yield return new Tuple<int, TreeNode>(3, this);
                foreach (Tuple<int, TreeNode> t in GetChildLines())
                {
                    yield return t;
                }
                yield return new Tuple<int, TreeNode>(1, this);
            }
            else
                yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}
