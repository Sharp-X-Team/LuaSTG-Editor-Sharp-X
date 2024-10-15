using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable, NodeIcon("returnnode.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class ReturnNode : TreeNode
    {
        [JsonConstructor]
        private ReturnNode() : base() { }

        public ReturnNode(DocumentData workSpaceData)
            : this(workSpaceData, "") { }

        public ReturnNode(DocumentData workSpaceData, string code) : base(workSpaceData)
        {
            Code = code;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Code")]
        public string Code
        {
            get => DoubleCheckAttr(0, name: "Code").attrInput;
            set => DoubleCheckAttr(0, name: "Code").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"return{(!string.IsNullOrEmpty(NonMacrolize(0)) ? $" {NonMacrolize(0)}" : "")}\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Return{(!string.IsNullOrEmpty(NonMacrolize(0)) ? $" {NonMacrolize(0)}" : "")}";
        }

        public override object Clone()
        {
            ReturnNode n = new(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
