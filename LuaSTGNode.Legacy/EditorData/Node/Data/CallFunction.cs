using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Data
{
    [Serializable, NodeIcon("callfunc.png")]
    [CreateInvoke(0), RCInvoke(1)]
    public class CallFunction : TreeNode
    {
        [JsonConstructor]
        private CallFunction() : base() { }

        public CallFunction(DocumentData workSpaceData)
            : this(workSpaceData, "", "") { }

        public CallFunction(DocumentData workSpaceData, string name, string param) : base(workSpaceData)
        {
            FuncName = name;
            Parameters = param;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Name")]
        public string FuncName
        {
            get => DoubleCheckAttr(0, name: "Name").attrInput;
            set => DoubleCheckAttr(0, name: "Name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Parameters")]
        public string Parameters
        {
            get => DoubleCheckAttr(1, name: "Parameters").attrInput;
            set => DoubleCheckAttr(1, name: "Parameters").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"{NonMacrolize(0)}({NonMacrolize(1)})\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Call function {NonMacrolize(0)} with paramaters: ({NonMacrolize(1)})";
        }

        public override object Clone()
        {
            TreeNode t = new CallFunction(parentWorkSpace);
            t.DeepCopyFrom(this);
            return t;
        }
    }
}
