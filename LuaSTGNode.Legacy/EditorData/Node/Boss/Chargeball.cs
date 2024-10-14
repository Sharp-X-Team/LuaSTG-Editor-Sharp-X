using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Enemy
{
    [Serializable, NodeIcon("chargeball.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class Chargeball : TreeNode
    {
        [JsonConstructor]
        public Chargeball() : base() { }

        public Chargeball(DocumentData workSpaceData) : this(workSpaceData, "self.x, self.y", "60", "80", "360", "1", "300", "255, 64, 64", "1.5") { }

        public Chargeball(DocumentData workSpaceData, string pos, string ktime, string ctime, string rad, string way, string ang, string col, string rotv) : base(workSpaceData)
        {
            Position = pos;
            KeepTime = ktime;
            ContractTime = ctime;
            Radius = rad;
            Ways = way;
            Angle = ang;
            Color = col;
            RotVel = rotv;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(0, "position", "Position").attrInput;
            set => DoubleCheckAttr(0, "position", "Position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string KeepTime
        {
            get => DoubleCheckAttr(1, name: "Keep Time").attrInput;
            set => DoubleCheckAttr(1, name: "Keep Time").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ContractTime
        {
            get => DoubleCheckAttr(2, name: "Contract Time").attrInput;
            set => DoubleCheckAttr(2, name: "Contract Time").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Radius
        {
            get => DoubleCheckAttr(3, name: "Radius").attrInput;
            set => DoubleCheckAttr(3, name: "Radius").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Ways
        {
            get => DoubleCheckAttr(4, name: "Ways").attrInput;
            set => DoubleCheckAttr(4, name: "Ways").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Angle
        {
            get => DoubleCheckAttr(5, name: "Angle").attrInput;
            set => DoubleCheckAttr(5, name: "Angle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Color
        {
            get => DoubleCheckAttr(6, "RGB", "Color").attrInput;
            set => DoubleCheckAttr(6, "RGB", "Color").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string RotVel
        {
            get => DoubleCheckAttr(7, name: "Rotation Velocity").attrInput;
            set => DoubleCheckAttr(7, name: "Rotation Velocity").attrInput = value;
        }


        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "New(boss_cast_darkball, " + Macrolize(0) + ", " + Macrolize(1) + ", " + Macrolize(2) + ", " + Macrolize(3)
                + ", " + Macrolize(4) + ", " + Macrolize(5) + ", " + Macrolize(6) + ", " + Macrolize(7) + ")\n";
        }

        public override string ToString()
        {
            return "Make chargeball in (" + NonMacrolize(0) + "), keep " + NonMacrolize(1) + " frame(s), contract in " + NonMacrolize(2)
                + " frame(s), in radius " + NonMacrolize(3) + ", with " + NonMacrolize(4) + " way(s), angle = " + NonMacrolize(5)
                + ", color = (" + NonMacrolize(6) + "), rotation velocity = " + NonMacrolize(7) + "";
        }

        public override object Clone()
        {
            var n = new Chargeball(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}
