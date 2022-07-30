using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("callbackfunc.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(DefineItem))]
    [RCInvoke(0)]
    public class ItemOnColli : TreeNode, ICallBackFunc
    {
        [JsonConstructor]
        private ItemOnColli() : base() { }

        public ItemOnColli(DocumentData workSpaceData)
            : base(workSpaceData)
        {
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0));
            yield return sp + "_editor_class[\"" + parentName + "\"].colli" + "=function(self, other)\n"
                        + sp + s1 + "if other == player then\n";
            foreach (var a in base.ToLua(spacing + 2))
            {
                yield return a;
            }
            yield return sp + s1 + "end\n"
                        + sp + "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "(Item) on colli()";
        }

        public override object Clone()
        {
            var n = new ItemOnColli(parentWorkSpace);
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

        [JsonIgnore]
        public string FuncName
        {
            get => Macrolize(0);
        }
    }
}
