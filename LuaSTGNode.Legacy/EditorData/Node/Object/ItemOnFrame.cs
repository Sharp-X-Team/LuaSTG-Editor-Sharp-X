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
    public class ItemOnFrame : TreeNode, ICallBackFunc
    {
        [JsonConstructor]
        private ItemOnFrame() : base() { }

        public ItemOnFrame(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Event type", ev, this, "event"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0));
            yield return sp + "_editor_class[\"" + parentName + "\"].frame = function(self)\n"
                + sp + s1 + "local player = self.target\n"
                + sp + s1 + "if self.timer < 24 then\n"
                + sp + s1 + s1 + "self.rot = self.rot + 45\n"
                + sp + s1 + s1 + "self.hscale = (self.timer + 25) / 48\n"
                + sp + s1 + s1 + "self.vscale = self.hscale\n"
                + sp + s1 + s1 + "if self.timer == 22 then\n"
                + sp + s1 + s1 + s1 + "self.vy = min(self.v, 2)\n"
                + sp + s1 + s1 + s1 + "self.vx = 0\n"
                + sp + s1 + s1 + "end\n"
                + sp + s1 + "elseif self.attract > 0 then\n"
                + sp + s1 + s1 + "local a = Angle(self, player)\n"
                + sp + s1 + s1 + "self.vx = self.attract * cos(a) + player.dx * 0.5\n"
                + sp + s1 + s1 + "self.vy = self.attract * sin(a) + player.dy * 0.5\n"
                + sp + s1 + "else\n"
                + sp + s1 + s1 + "self.vy = max(self.dy - 0.03, -1.7)\n"
                + sp + s1 + "end\n"
                + sp + s1 + "if self.y < lstg.world.boundb then\n"
                + sp + s1 + s1 + "_del(self, false)\n"
                + sp + s1 + "end\n"
                + sp + s1 + "if self.attract >= 8 then\n"
                + sp + s1 + s1 + "self.collected = true\n"
                + sp + s1 + "end\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(23, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "(Item) On frame()";
        }

        public override object Clone()
        {
            var n = new ItemOnFrame(parentWorkSpace);
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