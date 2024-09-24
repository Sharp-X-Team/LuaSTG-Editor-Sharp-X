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
    [RequireParent(typeof(BackgroundDefine)), Uniqueness]
    [RCInvoke(0)]
    public class BackgroundInit : TreeNode
    {
        [JsonConstructor]
        private BackgroundInit() : base() { }

        public BackgroundInit(DocumentData workSpaceData)
            : this(workSpaceData, "self", "false") { }

        public BackgroundInit(DocumentData workSpaceData, string col, string sc)
            : base(workSpaceData)
        {
            Target = col;
            Spellc = sc;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, name: "Target").attrInput;
            set => DoubleCheckAttr(0, name: "Target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Spellc
        {
            get => DoubleCheckAttr(1, "bool", "Is Spell Card BG").attrInput;
            set => DoubleCheckAttr(1, "bool", "Is Spell Card BG").attrInput = value;
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
            yield return sp + $"_editor_class[\"{parentName}\"].init = function(self)\n" +
                s1 + $"background.init({Macrolize(0)}, {Macrolize(1)})\n";
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
            return "(BG) On init()";
        }

        public override object Clone()
        {
            var n = new BackgroundInit(parentWorkSpace);
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
