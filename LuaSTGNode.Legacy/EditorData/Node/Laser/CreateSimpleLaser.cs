using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Laser
{
    [Serializable, NodeIcon("lasersimple.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [RCInvoke(0)]
    class CreateSimpleLaser : TreeNode
    {
        [JsonConstructor]
        public CreateSimpleLaser() : base() { }

        public CreateSimpleLaser(DocumentData workSpaceData)
            : this(workSpaceData, "self.x, self.y", "COLOR_RED", "0", "64", "32", "64", "8", "0", "0") { }

        public CreateSimpleLaser(DocumentData workSpaceData, string position, string color, string rot, string l1, string l2, string l3,
            string width, string node, string head)
            : base(workSpaceData)
        {
            Position = position;
            Color = color;
            Rotation = rot;
            HeadLength = l1;
            BodyLength = l2;
            TailLength = l3;
            Width = width;
            NodeSize = node;
            HeadSize = head;
        }

        public override object Clone()
        {
            CreateSimpleLaser n = new(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            bool hasChildren = Children.Count > 0;
            return $"Create simple laser{(hasChildren ? " with task" : "")} at ({NonMacrolize(0)})";
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
        public string Rotation
        {
            get => DoubleCheckAttr(2, "color").attrInput;
            set => DoubleCheckAttr(2, "color").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string HeadLength
        {
            get => DoubleCheckAttr(3, "length", "Head Length").attrInput;
            set => DoubleCheckAttr(3, "length", "Head Length").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string BodyLength
        {
            get => DoubleCheckAttr(4, "length", "Body Length").attrInput;
            set => DoubleCheckAttr(4, "length", "Body Length").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string TailLength
        {
            get => DoubleCheckAttr(5, "length", "Tail Length").attrInput;
            set => DoubleCheckAttr(5, "length", "Tail Length").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Width
        {
            get => DoubleCheckAttr(6, "length").attrInput;
            set => DoubleCheckAttr(6, "length").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string NodeSize
        {
            get => DoubleCheckAttr(7, name: "Node size").attrInput;
            set => DoubleCheckAttr(7, name: "Node size").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string HeadSize
        {
            get => DoubleCheckAttr(8, name: "Head size").attrInput;
            set => DoubleCheckAttr(8, name: "Head size").attrInput = value;
        }

        #endregion

        public override IEnumerable<string> ToLua(int spacing)
        {
            bool hasChildren = Children.Count > 0;
            string sp = Indent(spacing);
            string sp1 = Indent(spacing + 1);
            yield return sp + $"last = New(laser, {Macrolize(1)}, {Macrolize(0)}, {Macrolize(2)}, " +
                $"{Macrolize(3)}, {Macrolize(4)}, {Macrolize(5)}, {Macrolize(6)}, {Macrolize(7)}, {Macrolize(8)})\n";
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
