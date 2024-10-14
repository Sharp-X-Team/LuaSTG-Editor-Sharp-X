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
    [Serializable, NodeIcon("pactrometer.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class EnemyCharge : TreeNode
    {
        [JsonConstructor]
        public EnemyCharge() : base() { }

        public EnemyCharge(DocumentData workSpaceData) : this(workSpaceData, "self.x, self.y", "360", "255, 0, 0", "60", "2", "false") { }

        public EnemyCharge(DocumentData workSpaceData, string pos, string rad, string col, string time, string _mode, string _snd) : base(workSpaceData)
        {
            Position = pos;
            Radius = rad;
            Color = col;
            Time = time;
            Mode = _mode;
            Mute = _snd;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(0, "position", "Position").attrInput;
            set => DoubleCheckAttr(0, "position", "Position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Radius
        {
            get => DoubleCheckAttr(1, name: "Radius").attrInput;
            set => DoubleCheckAttr(1, name: "Radius").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Color
        {
            get => DoubleCheckAttr(2, "RGB", "Color").attrInput;
            set => DoubleCheckAttr(2, "RGB", "Color").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(3, name: "Time").attrInput;
            set => DoubleCheckAttr(3, name: "Time").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Mode
        {
            get => DoubleCheckAttr(4, name: "Mode").attrInput;
            set => DoubleCheckAttr(4, name: "Mode").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Mute
        {
            get => DoubleCheckAttr(5, "bool", "Mute").attrInput;
            set => DoubleCheckAttr(5, "bool", "Mute").attrInput = value;
        }


        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string[] macarray = new string[6];
            //
            for (int i = 0; i <= 5; i++) {
                if (string.IsNullOrEmpty(NonMacrolize(i))) {
                    macarray[i] = "";
                }
                else {
                    macarray[i] = Macrolize(i) + ((i == 5 || string.IsNullOrEmpty(NonMacrolize(i + 1))) ? "" : ", ");
                }
            }
            //

            yield return sp + "New(boss_cast_ef, " + macarray[0] + macarray[1] + macarray[2] + macarray[3] + macarray[4] + macarray[5] + ")\n";
       
        }

        public override string ToString()
        {
            return "Charge in (" + NonMacrolize(0) + "), with " + NonMacrolize(1) + " radius and color (" + NonMacrolize(2)
                + ") on " + NonMacrolize(3) + " frame(s) (mode is " + NonMacrolize(4) + ", muted is " + NonMacrolize(5) + ")";
        }

        public override object Clone()
        {
            var n = new EnemyCharge(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}
