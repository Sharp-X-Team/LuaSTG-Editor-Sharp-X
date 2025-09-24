﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;

namespace LuaSTGEditorSharp
{
    public static class NodesConfig
    {
        public static MetaModel[] SysImage =
            SystemMetaLoader.FromResource("pack://application:,,,/LuaSTGNode.Berry;component/ResourceList/SysImage.json");
        public static MetaModel[] SysImageGroup =
            SystemMetaLoader.FromResource("pack://application:,,,/LuaSTGNode.Berry;component/ResourceList/SysImageGroup.json");
        public static MetaModel[] SysSE =
            SystemMetaLoader.FromResource("pack://application:,,,/LuaSTGNode.Berry;component/ResourceList/SysSE.json");
        public static MetaModel[] SysTTF =
            SystemMetaLoader.FromResource("pack://application:,,,/LuaSTGNode.Berry;component/ResourceList/SysTTF.json");
        public static MetaModel[] SysFont =
            SystemMetaLoader.FromResource("pack://application:,,,/LuaSTGNode.Berry;component/ResourceList/SysFont.json");

        public static int MetaInfoCollectionTypeCount = (int)MetaType.__max;
    }
}
