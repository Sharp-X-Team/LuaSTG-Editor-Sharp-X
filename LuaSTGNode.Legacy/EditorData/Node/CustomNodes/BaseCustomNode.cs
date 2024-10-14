using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using MoonSharp.Interpreter;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.ComponentModel;
using LuaSTGEditorSharp.EditorData.Message;

namespace LuaSTGEditorSharp.EditorData.Node.CustomNodes
{
    [Serializable, NodeIcon("basecustomnode.png")]
    [CustomNode]
    [IgnoreValidation]
    //[CreateInvoke(0)]
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

            // Tries to read the "isLeaf" property, if not found, won't be a leaf node by default.
            try { isCustomNodeLeaf = nodeProperties.Table.Get("isLeaf").Boolean; }
            catch (System.Exception) { isCustomNodeLeaf = false; }

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
                string head = "", body = $"-- No code for {NodeFilePath}", tail = "";
                string[] parameters = GetParameters().ToArray();
                string sp = Indent(spacing);
                DynValue f_Head = GetLuaFunc("ToLuaHead");
                DynValue f_Body = GetLuaFunc("ToLuaBody");
                DynValue f_Tail = GetLuaFunc("ToLuaTail");
                if (!f_Head.IsNil()) head = NodeScript.Call(f_Head).String;
                if (!f_Body.IsNil()) body = NodeScript.Call(f_Body).String;
                if (!f_Tail.IsNil()) tail = NodeScript.Call(f_Tail).String;

                try
                {
                    head = sp + string.Format(head, parameters) + "\n";
                    body = sp + string.Format(body, parameters) + "\n";
                    tail = sp + string.Format(tail, parameters) + "\n";
                }
                catch {}
                if (head != sp + "\n") yield return head;
                if (body != sp + "\n") yield return body;
                foreach (var a in base.ToLua(spacing + 1))
                {
                    yield return a;
                }
                if (tail != sp + "\n") yield return tail;
            }
            else
            {
                foreach (var a in base.ToLua(spacing + 1))
                {
                    yield return a;
                }
            }
        }

        public override string ToString()
        {
            if (!GenerateScript()) return "* Unknown Node *";
            string NoText = "No Node description set for (" + nodeProperties.Table.Get("name") + "). Please set one.";
            DynValue f_ToString = NodeScript.Globals.Get("ToString");
            if (f_ToString.IsNil()) return NoText;
            DynValue DescString = NodeScript.Call(f_ToString);
            if (DescString.IsNil()) return NoText;

            List<string> parameters = GetParameters();
            return string.Format(DescString.String, parameters.ToArray());
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

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            if (GenerateScript())
            {
                string head = "", body = $"-- No code for {NodeFilePath}", tail = "";
                DynValue f_Head = GetLuaFunc("ToLuaHead");
                DynValue f_Body = GetLuaFunc("ToLuaBody");
                DynValue f_Tail = GetLuaFunc("ToLuaTail");
                if (!f_Head.IsNil()) head = NodeScript.Call(f_Head).String;
                if (!f_Body.IsNil()) body = NodeScript.Call(f_Body).String;
                if (!f_Tail.IsNil()) tail = NodeScript.Call(f_Tail).String;
                if (!string.IsNullOrEmpty(head)) yield return new Tuple<int, TreeNode>(head.Count((c) => c == '\n')+1, this);
                if (!string.IsNullOrEmpty(body)) yield return new Tuple<int, TreeNode>(body.Count((c) => c == '\n')+1, this);
                foreach (Tuple<int, TreeNode> t in GetChildLines())
                {
                    yield return t;
                }
                if (!string.IsNullOrEmpty(tail)) yield return new Tuple<int, TreeNode>(tail.Count((c) => c == '\n')+1, this);
            }
            else
            {
                yield return new Tuple<int, TreeNode>(1, this);
                foreach (Tuple<int, TreeNode> t in GetChildLines())
                {
                    yield return t;
                }
            }
        }

        public override object Clone()
        {
            var n = new BaseCustomNode(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public List<string> GetParameters()
        {
            List<string> parameters = new List<string>();
            for (int i = 0; i < attributes.Count; i++)
            {
                parameters.Add(NonMacrolize(i));
            }
            return parameters;
        }

        public DynValue GetLuaFunc(string func)
        {
            return NodeScript.Globals.Get(func);
        }
    }
}
