using System;
using System.IO;
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
using LuaSTGEditorSharp.CustomNodes;
using System.ComponentModel;
using LuaSTGEditorSharp.EditorData.Message;

namespace LuaSTGEditorSharp.EditorData.Node.CustomNodes
{
    [Serializable, NodeIcon("basecustomnode.png")]
    [CustomNode]
    [IgnoreValidation]
    [CreateInvoke(0)]
    [IgnoreAttributesParityCheck]
    public class BaseCustomNode : TreeNode
    {
        [JsonIgnore, XmlIgnore]
        protected override bool EnableParityCheck => false;

        [JsonIgnore, XmlIgnore]
        private DynValue nodeProperties;
        [JsonIgnore, XmlIgnore]
        private Script NodeScript;

        [JsonIgnore, XmlIgnore]
        private string eNodeFilePath;

        [JsonProperty, DefaultValue("NullNode")]
        [XmlAttribute("nodeScript")]
        public string NodeFilePath
        {
            get => eNodeFilePath;
            set => eNodeFilePath = value;
        }

        [JsonConstructor]
        private BaseCustomNode() : base() { }

        public BaseCustomNode(DocumentData workSpaceData)
            : this(workSpaceData, "NullNode") { }

        public BaseCustomNode(DocumentData workSpaceData, string nodeFilePath)
            : base(workSpaceData)
        {
            isCustomNode = true;
            NodeFilePath = nodeFilePath;
            NodeScript = new Script();
            if (NodeFilePath != "NullNode" && File.Exists(@"CustomNodes/" + NodeFilePath + ".lua"))
                NodeScript.DoFile(@"CustomNodes/" + NodeFilePath + ".lua");

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

        /// <summary>
        /// Regenerate the script's lua code at runtime.
        /// Is needed for runtime script edits and not crashing the editor when it starts (for some reason).
        /// </summary>
        /// <returns>If the code was parsed correctly returns true, returns false if it's not the case.</returns>
        public bool GenerateScript()
        {
            if (nodeProperties != null) return true;
            NodeScript = new Script();
            if (NodeFilePath != "NullNode" && File.Exists(@"CustomNodes/" + NodeFilePath + ".lua"))
                NodeScript.DoFile(@"CustomNodes/" + NodeFilePath + ".lua");

            DynValue f_Init = NodeScript.Globals.Get("InitNode");
            if (f_Init.IsNil()) return false;
            nodeProperties = NodeScript.Call(f_Init);
            return true;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            if (GenerateScript())
            {
                string sp = Indent(spacing);
                yield return sp + "" + Macrolize(0) + ".group = " + Macrolize(1) + "\n";
            }
            else
            {
                foreach (var a in base.ToLua(spacing + 1))
                {
                    yield return a;
                }
            }
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            GenerateScript();
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            if (!GenerateScript()) return "* Unknown Node *";
            string NoText = "No Node description set for (" + nodeProperties.Table.Get("name") + "). Please set one.";
            DynValue f_ToString = NodeScript.Globals.Get("ToString");
            if (f_ToString.IsNil()) return NoText;
            DynValue DescString = NodeScript.Call(f_ToString);
            if (DescString.IsNil()) return NoText;

            List<string> parameters = new List<string>();
            for (int i = 2; i < DescString.Table.Length+1; i++)
            {
                int attrId = 0;
                Table _parameters = nodeProperties.Table.Get("Parameters").Table;
                if (_parameters == null) return "NaN";
                for (int j = 1; j < _parameters.Length+1; j++)
                {
                    if (_parameters.Get(j).Table[1].ToString() == DescString.Table[i].ToString())
                    {
                        attrId = j-1;
                        break;
                    }
                }
                parameters.Add(NonMacrolize(attrId).ToString());
            }
            return string.Format(DescString.Table[1].ToString(), parameters.ToArray());
            //return "Set " + NonMacrolize(0) + "\'s group to " + NonMacrolize(1) + "";
        }

        /// <summary>
        /// Overriden function, returns a <see cref="InvalidCustomNodeMessage"/> error when the node doesn't exist in the "CustomNodes" folder.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of error messages.</returns>
        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (NodeFilePath != "NullNode")
            {
                if (!File.Exists(@"CustomNodes/" + NodeFilePath + ".lua"))
                {
                    messages.Add(new InvalidCustomNodeMessage(NodeFilePath, this));
                }
            }
            return messages;
        }

        public override object Clone()
        {
            var n = new BaseCustomNode(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
