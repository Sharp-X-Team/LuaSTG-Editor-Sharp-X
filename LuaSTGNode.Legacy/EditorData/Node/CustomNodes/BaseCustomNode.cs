using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using MoonSharp.Interpreter;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Runtime.Remoting.Messaging;

namespace LuaSTGEditorSharp.EditorData.Node.CustomNodes
{
    [Serializable, NodeIcon("objectgroup.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class BaseCustomNode : TreeNode
    {
        [JsonIgnore, XmlIgnore]
        private DynValue nodeProperties;

        [JsonIgnore, XmlIgnore]
        public Script NodeScript;

        [JsonConstructor]
        private BaseCustomNode() : base() { }

        public BaseCustomNode(DocumentData workSpaceData)
            : this(workSpaceData, new Script()) { } // TODO: Changer le "null" en un truc qui va chercher les paramètres de base du node

        public BaseCustomNode(DocumentData workSpaceData, Script nodeScript)
            : base(workSpaceData)
        {
            isCustomNode = true;

            NodeScript = nodeScript;
            DynValue f_Init = NodeScript.Globals.Get("InitNode");
            if (f_Init.IsNil()) return;
            nodeProperties = NodeScript.Call(f_Init);

            Table parameters = nodeProperties.Table.Get("Parameters").Table;
            if (parameters == null) return;

            for (int i = 1; i < parameters.Length+1; i++)
            {
                attributes.Add(new AttrItem(parameters.Get(i).Table[1].ToString(), parameters.Get(i).Table[2].ToString(), this, parameters.Get(i).Table[3].ToString()));
            }
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "" + Macrolize(0) + ".group = " + Macrolize(1) + "\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set " + NonMacrolize(0) + "\'s group to " + NonMacrolize(1) + "";
        }

        public override List<MessageBase> GetMessage()
        {
            return new List<MessageBase>();
        }

        public override object Clone()
        {
            var n = new BaseCustomNode(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
