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

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("onrender.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(PlayerDefine)), Uniqueness]
    [RCInvoke(0)]
    public class PlayerRender : TreeNode
    {
        [JsonConstructor]
        private PlayerRender() : base() { }

        public PlayerRender(DocumentData workSpaceData) : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Parameter List", para, this));
            attributes.Add(new AttrItem("Image", image, this, "image"));
            attributes.Add(new AttrItem("Layer", layer, this, "layer"));
            attributes.Add(new AttrItem("Group", group, this, "group"));
            attributes.Add(new AttrItem("Hide", hide, this, "bool"));
            attributes.Add(new AttrItem("Bound", bound, this, "bool"));
            attributes.Add(new AttrItem("Auto Rotation", autorot, this, "bool"));
            attributes.Add(new AttrItem("HP", hp, this));
            attributes.Add(new AttrItem("Collision", colli, this, "bool"));
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
            yield return sp + "" + parentName + ".render = function(self)\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(13, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "On render(" + NonMacrolize(0) + ")";
        }

        public override object Clone()
        {
            var n = new PlayerRender(parentWorkSpace);
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
