using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Bullet
{
    [Serializable, NodeIcon("bulletinit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(PlayerBulletDefine)), Uniqueness]
    [RCInvoke(0)]
    public class PlayerBulletInit : TreeNode
    {
        [JsonConstructor]
        private PlayerBulletInit() : base() { }

        public PlayerBulletInit(DocumentData workSpaceData)
            : this(workSpaceData, "", "\"leaf\"", "9", "90", "2") { }

        public PlayerBulletInit(DocumentData workSpaceData, string par, string img, string vel, string ang, string dmg)
            : base(workSpaceData)
        {
            Parameters = par;
            Image = img;
            Velocity = vel;
            Angle = ang;
            Damage = dmg;
            /*
            attributes.Add(new AttrItem("Parameter List", para, this));
            attributes.Add(new AttrItem("Style", style, this, "bulletStyle"));
            attributes.Add(new AttrItem("Color", color, this, "color"));
            attributes.Add(new AttrItem("Stay on create", "true", this, "bool"));
            attributes.Add(new AttrItem("Destroyable", "true", this, "bool"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Parameters
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(1, "objimage", "Image").attrInput;
            set => DoubleCheckAttr(1, "objimage", "Image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Velocity
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

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            if (Parent?.attributes != null && Parent.AttributeCount >= 1)
            {
                parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0));
            }
            string p = (!string.IsNullOrEmpty(NonMacrolize(0)) ? NonMacrolize(0) : "_");
            yield return sp + "_editor_class[\"" + parentName + "\"].init=function(self,_x,_y," + p + ")\n"
                         + sp + s1 + "player_bullet_straight.init(self," + Macrolize(1) + ",_x,_y," + Macrolize(2) + "," + Macrolize(3) + "," + Macrolize(4) + ")\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(3, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "on init(" + NonMacrolize(0) + ")";
        }

        public override object Clone()
        {
            var n = new PlayerBulletInit(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            TreeNode p = GetLogicalParent();
            if (p?.attributes == null || p.AttributeCount < 1)
            {
                a.Add(new CannotFindAttributeInParent(1, this));
            }
            return a;
        }
    }
}
