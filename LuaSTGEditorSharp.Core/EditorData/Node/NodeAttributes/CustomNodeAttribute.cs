using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.NodeAttributes
{
    /// <summary>
    /// Identify a <see cref="TreeNode"/> as being a custom node.
    /// This node's icon will load differently than normal ones.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomNodeAttribute : Attribute
    {
    }
}
