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

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("playersimplebullet.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class PlayerSimpleBullet : TreeNode
    {
        [JsonConstructor]
        private PlayerSimpleBullet() : base() { }

        public PlayerSimpleBullet(DocumentData workSpaceData)
            : this(workSpaceData, "\"leaf\"", "self.x,self.y", "9", "90", "2", "0") { }

        public PlayerSimpleBullet(DocumentData workSpaceData, string img, string pos, string spd, string ang, string dmg, string home)
            : base(workSpaceData)
        {
            Image = img;
            Position = pos;
            Speed = spd;
            Angle = ang;
            Damage = dmg;
            Homing = home;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(0, "objimage").attrInput;
            set => DoubleCheckAttr(0, "objimage").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(1, "position").attrInput;
            set => DoubleCheckAttr(1, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Speed
        {
            get => DoubleCheckAttr(2).attrInput;
            set => DoubleCheckAttr(2).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Angle
        {
            get => DoubleCheckAttr(3).attrInput;
            set => DoubleCheckAttr(3).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Damage
        {
            get => DoubleCheckAttr(4).attrInput;
            set => DoubleCheckAttr(4).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Homing
        {
            get => DoubleCheckAttr(5, name: "Homing Strength").attrInput;
            set => DoubleCheckAttr(5, name: "Homing Strength").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "last=New(player_bullet_trail," + Macrolize(0) + "," + Macrolize(1) + "," +
                               Macrolize(2) + "," + Macrolize(3) + ",player.target," + Macrolize(5) + "," + Macrolize(4) + ")\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Create simple player bullet at (" + NonMacrolize(1) + ") with image " + NonMacrolize(0) +
                ", speed and angle is (" + NonMacrolize(2) + "," + NonMacrolize(3) + "), damage is (" + NonMacrolize(4) + ") (homing strength is " + NonMacrolize(5) + ")";
        }

        public override object Clone()
        {
            var n = new PlayerSimpleBullet(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
