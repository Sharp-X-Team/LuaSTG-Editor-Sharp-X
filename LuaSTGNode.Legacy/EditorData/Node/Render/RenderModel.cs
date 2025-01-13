using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Render
{
    [Serializable, NodeIcon("rendermodel3d.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [ RCInvoke(0)]
    public class RenderModel : TreeNode
    {
        [JsonConstructor]
        private RenderModel() : base() { }

        public RenderModel(DocumentData workSpaceData)
            : this(workSpaceData, "", "0, 0, 0", "0", "0", "0", "1, 1, 1") { }

        public RenderModel(DocumentData workSpaceData, string modname, string pos, string roll, string pitch, string yaw,
            string scale)
            : base(workSpaceData)
        {
            ModelName = modname;
            Position = pos;
            Roll = roll;
            Pitch = pitch;
            Yaw = yaw;
            Scale = scale;
        }

        [JsonIgnore, NodeAttribute]
        public string ModelName
        {
            get => DoubleCheckAttr(0, "model", "Model Name").attrInput;
            set => DoubleCheckAttr(0, "model", "Model Name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(1, name: "Position 3D").attrInput;
            set => DoubleCheckAttr(1, name: "Position 3D").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Roll
        {
            get => DoubleCheckAttr(2).attrInput;
            set => DoubleCheckAttr(2).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Pitch
        {
            get => DoubleCheckAttr(3).attrInput;
            set => DoubleCheckAttr(3).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Yaw
        {
            get => DoubleCheckAttr(4).attrInput;
            set => DoubleCheckAttr(4).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Scale
        {
            get => DoubleCheckAttr(5, name: "Scale 3D").attrInput;
            set => DoubleCheckAttr(5, name: "Scale 3D").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"lstg.RenderModel({Macrolize(0)}, {Macrolize(1)}, {Macrolize(2)}, {Macrolize(3)}, {Macrolize(4)}, {Macrolize(5)})\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Render model \"{NonMacrolize(0)}\" at ({NonMacrolize(1)}) with (Roll = {NonMacrolize(2)}), (Pitch = {NonMacrolize(3)}), (Yaw = {NonMacrolize(4)}) and scale ({NonMacrolize(5)})";
        }

        public override object Clone()
        {
            RenderModel n = new(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
