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
    [Serializable, NodeIcon("renderanim.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class RenderAnimation : TreeNode
    {
        [JsonConstructor]
        private RenderAnimation() : base() { }

        public RenderAnimation(DocumentData workSpaceData)
            : this(workSpaceData, "", "1", "self.x, self.y", "0", "1, 1", "1") { }

        public RenderAnimation(DocumentData workSpaceData, string aniname, string anitimer, string pos, string rot, string scale, string z)
            : base(workSpaceData)
        {
            Name = aniname;
            Timer = anitimer;
            Position = pos;
            Rotation = rot;
            Scale = scale;
            Z = z;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0, "objimage", "Animation").attrInput;
            set => DoubleCheckAttr(0, "objimage", "Animation").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Timer
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(2, "position", "Position").attrInput;
            set => DoubleCheckAttr(2, "position", "Position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Rotation
        {
            get => DoubleCheckAttr(3).attrInput;
            set => DoubleCheckAttr(3).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Scale
        {
            get => DoubleCheckAttr(4).attrInput;
            set => DoubleCheckAttr(4).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Z
        {
            get => DoubleCheckAttr(5).attrInput;
            set => DoubleCheckAttr(5).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"lstg.RenderAnimation({Macrolize(0)}, {Macrolize(1)}, {Macrolize(2)}, {Macrolize(3)}, {Macrolize(4)}, {Macrolize(5)})\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Render animation {NonMacrolize(0)} with timer " + NonMacrolize(1) + "\n" +
                $"Position = ({NonMacrolize(2)}) Rotation = ({NonMacrolize(3)}) Scale = ({NonMacrolize(4)}) Z = ({NonMacrolize(5)})";
        }

        public override object Clone()
        {
            RenderAnimation n = new(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
