using System.Collections.Generic;
using System.IO;
using System.Linq;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Plugin;
using MoonSharp.Interpreter;
using static LuaSTGEditorSharp.Plugin.AbstractToolbox;
using LuaSTGEditorSharp.EditorData.Node.CustomNodes;

namespace LuaSTGEditorSharp.CustomNodes
{
    public class CustomNodeLoader
    {
        public Script InitScript = new Script();
        public PluginToolbox pluginToolbox;

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
                Script tmp_node = new Script();
                tmp_node.DoFile(path);
                DynValue f_Init = tmp_node.Globals.Get("InitNode");
                DynValue node_properties = tmp_node.Call(f_Init);

                string tag = "cusNode_" + node_properties.Table.Get("name").String.ToLower();

                cnodes.Add(new ToolboxItemData(tag,
                            GetComponentImage(node_properties.Table.Get("image").String),
                            node_properties.Table.Get("name").String)
                    , new AddCustomNode(AddCustomNode));

                pluginToolbox.CustomScripts.Add(tag, NodesToLoad.Table[i].ToString());
            }
        }

        public string GetComponentImage(string FileName)
        {
            string img_name = "userdefinednode.png";
            if (!string.IsNullOrEmpty(FileName))
                img_name = FileName;
            return img_name;
        }

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
