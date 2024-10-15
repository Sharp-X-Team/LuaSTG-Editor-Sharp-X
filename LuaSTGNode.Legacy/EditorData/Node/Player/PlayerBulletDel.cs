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
    public class PlayerBulletDel : TreeNode
    {
        [JsonConstructor]
        private PlayerBulletDel() : base() { }

        public PlayerBulletDel(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Parameter List", para, this));
            attributes.Add(new AttrItem("Style", style, this, "bulletStyle"));
            attributes.Add(new AttrItem("Color", color, this, "color"));
            attributes.Add(new AttrItem("Stay on create", "true", this, "bool"));
            attributes.Add(new AttrItem("Destroyable", "true", this, "bool"));
            */
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
            yield return sp + "_editor_class[\"" + parentName + "\"].del = function(self)\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "On del()";
        }

        public override object Clone()
        {
            var n = new PlayerBulletDel(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
