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
    [Serializable, NodeIcon("bgonrender.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(BackgroundDefine))]
    [RCInvoke(0)]
    public class BGOnRender : TreeNode, ICallBackFunc
    {
        [JsonConstructor]
        public BGOnRender() : base() { }

        public BGOnRender(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0));
            yield return sp + "_editor_class[\"" + parentName + "\"].render=function(self)\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override string ToString()
        {
            return "(BG) on render()";
        }

        public override object Clone()
        {
            var n = new BGOnRender(parentWorkSpace);
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
