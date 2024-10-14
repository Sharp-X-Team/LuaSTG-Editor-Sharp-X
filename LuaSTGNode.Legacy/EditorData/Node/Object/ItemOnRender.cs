using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Node.Object;


namespace LuaSTGEditorSharp.EditorData.Node.Render
{
    [Serializable, NodeIcon("onrender.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(DefineItem))]
    [RCInvoke(0)]
    public class ItemOnRender : TreeNode, ICallBackFunc
    {
        [JsonConstructor]
        public ItemOnRender() : base() { }

        public ItemOnRender(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0));
            yield return sp + "_editor_class[\"" + parentName + "\"].render = function(self)\n"
                        + sp + s1 + "if self.y > lstg.world.t then\n"
                        + sp + s1 + s1 + "Render(self.imgup, self.x, lstg.world.t - 8)\n"
                        + sp + s1 + "else\n";
            foreach (var a in base.ToLua(spacing + 2))
            {
                yield return a;
            }
            yield return sp + s1 + "end\n"
                        + sp + "end\n";
        }

        public override string ToString()
        {
            return "(Item) On render()";
        }

        public override object Clone()
        {
            var n = new ItemOnRender(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in base.GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        [JsonIgnore]
        public string FuncName
        {
            get => "render";
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
