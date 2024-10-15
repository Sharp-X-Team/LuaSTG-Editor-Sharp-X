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
    [Serializable, NodeIcon("objectinit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(DefineItem)), Uniqueness]
    [RCInvoke(0)]
    public class ItemInit : TreeNode
    {
        [JsonConstructor]
        private ItemInit() : base() { }

        public ItemInit(DocumentData workSpaceData)
            : this(workSpaceData, "", "\"leaf\"", "\"leaf\"") { }

        public ItemInit(DocumentData workSpaceData, string para, string image, string image_up)
            : base(workSpaceData)
        {
            ParameterList = para;
            Image = image;
            ImageUp = image_up;
        }

        [JsonIgnore, NodeAttribute]
        public string ParameterList
        {
            get => DoubleCheckAttr(0, name: "Parameter List").attrInput;
            set => DoubleCheckAttr(0, name: "Parameter List").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(1, "objimage").attrInput;
            set => DoubleCheckAttr(1, "objimage").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ImageUp
        {
            get => DoubleCheckAttr(2, "objimage").attrInput;
            set => DoubleCheckAttr(2, "objimage").attrInput = value;
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
            yield return sp + "_editor_class[\"" + parentName + "\"].init = function(self, _x, _y, " + p + ")\n"
                        + sp + s1 + "x = min(max(_x, lstg.world.l + 8), lstg.world.r - 8)\n"
                        + sp + s1 + "self.x, self.y = x, _y\n"
                        + sp + s1 + "angle = angle or 90\n"
                        + sp + s1 + "v = v or 1.5\n"
                        + sp + s1 + "SetV(self, v, angle)\n"
                        + sp + s1 + "self.v = v\n"
                        + sp + s1 + "self.group = GROUP_ITEM\n"
                        + sp + s1 + "self.layer = LAYER_ITEM\n"
                        + sp + s1 + "self.bound = false\n"
                        + sp + s1 + "self.img = " + Macrolize(1) + "\n"
                        + sp + s1 + "self.imgup = " + Macrolize(2) + "\n"
                        + sp + s1 + "self.servants = {}\n"
                        + sp + s1 + "self.attract = 0\n"
                        + sp + s1 + "self._blend, self._a, self._r, self._g, self._b= '', 255, 255, 255, 255\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(15, this);
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
            var n = new ItemInit(parentWorkSpace);
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
