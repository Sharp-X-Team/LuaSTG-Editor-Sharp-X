using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp.CustomNodes
{
    public class CustomNodeRuntimeError : MessageBase
    {
        public string NodePath { get; set; }
        public override string SourceName { get => "Custom Node System"; }
        public override DocumentData SourceDoc { get => null; }

        public CustomNodeRuntimeError(string nodePath, IMessageThrowable source)
            : base(0, source)
        {
            NodePath = nodePath;
        }

        public override string ToString()
        {
            return $"A node with the path '{NodePath}' couldn't be loaded. (Runtime error)";
        }

        public override object Clone()
        {
            return new CustomNodeRuntimeError(NodePath, Source);
        }

        public override void Invoke()
        {

        }
    }
}
