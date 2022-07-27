using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("defaultaction.png")]
    [RequireAncestor(typeof(CallBackFunc), typeof(Data.Function), typeof(Render.OnRender), typeof(Bullet.PlayerBulletRender), typeof(Bullet.PlayerBulletFrame), typeof(Bullet.PlayerBulletColli), typeof(Bullet.PlayerBulletKill), typeof(Bullet.PlayerBulletDel), typeof(Render.ItemOnRender))]
    [LeafNode]
    public class DefaultAction : TreeNode
    {
        [JsonConstructor]
        private DefaultAction() : base() { }

        public DefaultAction(DocumentData workSpaceData)
            : this(workSpaceData, "") { }

        public DefaultAction(DocumentData workSpaceData, string code)
            : base(workSpaceData)
        {
            CodeAddon = code;
        }

        [JsonIgnore, NodeAttribute]
        public string CodeAddon
        {
            get => DoubleCheckAttr(0, "event", "Event type").attrInput;
            set => DoubleCheckAttr(0, "event", "Event type").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            TreeNode callBackFunc = this;
            if (!string.IsNullOrEmpty(NonMacrolize(0)))
            {
                yield return sp + "self.class.base." + Macrolize(0) + "(self)\n";
            }
            else
            {
                while (!(callBackFunc is ICallBackFunc) && callBackFunc != null)
                {
                    callBackFunc = callBackFunc.Parent;
                }
                ICallBackFunc func = (ICallBackFunc)callBackFunc;
                if (callBackFunc != null)
                {
                    string other = func.FuncName == "colli" ? ",other" : "";
                    yield return sp + "self.class.base." + func.FuncName + "(self" + other + ")\n";
                }
                else
                {
                    yield return "\n";
                }
            }
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Do default action";
        }

        public override object Clone()
        {
            var n = new DefaultAction(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            TreeNode callBackFunc = this;
            while (!(callBackFunc is ICallBackFunc) && callBackFunc != null)
            {
                callBackFunc = callBackFunc.Parent;
            }
            if (callBackFunc == null && string.IsNullOrEmpty(NonMacrolize(0))) 
            {
                a.Add(new CannotFindAncestorTypeOf("CallBackFunc", this));
            }
            return a;
        }
    }
}
