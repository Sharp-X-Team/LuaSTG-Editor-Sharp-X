using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Node.General;
using LuaSTGEditorSharp.EditorData.Node.Advanced;
using System.Windows;
using System.Windows.Resources;
using System.IO;
using LuaSTGEditorSharp.EditorData.Node.Tween;

namespace LuaSTGEditorSharp
{
    public class PluginToolbox : AbstractToolbox
    {
        public PluginToolbox(IMainWindow mw) : base(mw) { }

        public override void InitFunc()
        {
            var tween = new Dictionary<ToolboxItemData, AddNode>();
            #region data
            tween.Add(new ToolboxItemData("createTween", "/LuaSTGNode.Berry;component/images/createtween.png", "Create Tween")
                , new AddNode(AddCreateTweenNode));
            tween.Add(new ToolboxItemData("tweenease", "/LuaSTGNode.Berry;component/images/tweenease.png", "Ease Tween")
                , new AddNode(AddTweenEaseNode));
            #endregion
            ToolInfo.Add("Tweens", tween);
        }

        #region data

        private void AddCreateTweenNode()
        {
            parent.Insert(new CreateTween(parent.ActivatedWorkSpaceData));
        }

        private void AddTweenEaseNode()
        {
            parent.Insert(new TweenEase(parent.ActivatedWorkSpaceData));
        }

        #endregion
    }
}
