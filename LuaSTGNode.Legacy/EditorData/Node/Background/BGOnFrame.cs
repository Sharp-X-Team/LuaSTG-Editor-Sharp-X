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
    [RequireParent(typeof(BackgroundDefine))]
    [RCInvoke(0)]
    public class BGOnFrame : TreeNode, ICallBackFunc
    {
        [JsonConstructor]
        private BGOnFrame() : base() { }

        public BGOnFrame(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Event type", ev, this, "event"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0));
            yield return sp + $"_editor_class[\"{parentName}\"].frame = function(self)\n" + Indent(1) + "task.Do(self)\n";
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
            return "(BG) On frame()";
        }

        public override object Clone()
        {
            var n = new BGOnFrame(parentWorkSpace);
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
