using System.Collections.Generic;
using System.IO;
using System.Linq;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Plugin;
using MoonSharp.Interpreter;
using static LuaSTGEditorSharp.Plugin.AbstractToolbox;
using LuaSTGEditorSharp.EditorData.Node.CustomNodes;
using System;
using LuaSTGEditorSharp.EditorData.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LuaSTGEditorSharp.CustomNodes
{
    public class CustomNodeLoader : IMessageThrowable
    {
        public Script InitScript = new Script();
        public PluginToolbox pluginToolbox;

        private List<string> filesNotLoaded = new List<string>();
        private List<string> nodeRuntimeError = new List<string>();

        #region IMessageThrowable implementation

        public ObservableCollection<MessageBase> Messages { get; } = new ObservableCollection<MessageBase>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }

        public void CheckMessage(object sender, PropertyChangedEventArgs e)
        {
            var ms = GetMessage();
            Messages.Clear();
            foreach (MessageBase mb in ms)
            {
                Messages.Add(mb);
            }
            MessageContainer.UpdateMessage(this);
        }

        public List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();

            foreach (string file in filesNotLoaded)
                messages.Add(new CustomNodeSyntaxError(file, this));
            foreach (string file in nodeRuntimeError)
                messages.Add(new CustomNodeRuntimeError(file, this));

            return messages;
        }

        #endregion

        public CustomNodeLoader(PluginToolbox pT)
        {
            // This chunk of code loads the Init.lua file and call the InitNodes function to retrieve the nodes to load.
            pluginToolbox = pT;
            InitScript.DoFile(@"CustomNodes/Init.lua");
        }

        public void LoadCustomNodes(ref Dictionary<ToolboxItemData, AddCustomNode> cnodes)
        {
            var f_InitNodes = InitScript.Globals["InitNodes"];
            DynValue NodesToLoad = InitScript.Call(f_InitNodes);
            filesNotLoaded.Clear();
            nodeRuntimeError.Clear();

            PropertyChanged += new PropertyChangedEventHandler(CheckMessage);

            for (int i = 1; i < NodesToLoad.Table.Length + 1; i++)
            {
                if (NodesToLoad.Table[i].ToString() == "_Separator")
                {
                    cnodes.Add(new ToolboxItemData(true), null);
                    continue;
                }

                string path = @"CustomNodes/" + NodesToLoad.Table[i].ToString() + ".lua";
                if (!File.Exists(path))
                    continue;
                // Node loading from file.
                Script tmp_node = new Script();
                try
                {
                    tmp_node.DoFile(path);
                }
                catch (SyntaxErrorException)
                {
                    filesNotLoaded.Add(path);
                    RaisePropertyChanged("filesNotLoaded");
                    continue;
                }

                // Attempt to add the node to the toolbox. The only failure possible is a ScriptRuntimeException
                try
                {
                    DynValue f_Init = tmp_node.Globals.Get("InitNode");
                    if (f_Init.IsNil()) continue;
                    DynValue node_properties = tmp_node.Call(f_Init);

                    string tag = "cusNode_" + node_properties.Table.Get("name").String.ToLower();

                    cnodes.Add(new ToolboxItemData(tag,
                                GetComponentImage(node_properties.Table.Get("image").String),
                                node_properties.Table.Get("name").String)
                        , new AddCustomNode(AddCustomNode));

                    pluginToolbox.CustomScripts.Add(tag, NodesToLoad.Table[i].ToString());
                }
                catch (ScriptRuntimeException)
                {
                    nodeRuntimeError.Add(path);
                    RaisePropertyChanged("nodeRuntimeError");
                    continue;
                }
            }
        }

        public string GetComponentImage(string FileName)
        {
            string img_name = "userdefinednode.png";
            if (!string.IsNullOrEmpty(FileName))
                img_name = FileName;
            return img_name;
        }

        [Obsolete]
        public string GetCustomImage(string FileName)
        {
            string img_name = @"CustomNodes/Images/" + FileName + ".png";
            bool exists = File.Exists(img_name);
            if (!exists)
                img_name = "/LuaSTGEditorSharp.Core;component/images/userdefinednode.png"; // Default Icon when the image doesn't exists.
            return img_name;
        }

        private void AddCustomNode(string nodeScriptString)
        {
            TreeNode c_Node = new BaseCustomNode(pluginToolbox.parent.ActivatedWorkSpaceData, nodeScriptString);
            pluginToolbox.parent.Insert(c_Node);
        }
    }
}
