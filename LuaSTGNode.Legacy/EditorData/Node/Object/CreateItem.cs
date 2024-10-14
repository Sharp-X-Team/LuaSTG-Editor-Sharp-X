using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("createitem.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class CreateItem : TreeNode
    {
        [JsonConstructor]
        private CreateItem() : base() { }

        public CreateItem(DocumentData workSpaceData)
            : this(workSpaceData, "", "self.x, self.y", "false", "")
        { }

        public CreateItem(DocumentData workSpaceData, string name, string pos, string animate, string param)
            : base(workSpaceData)
        {
            Name = name;
            Position = pos;
            Animate = animate;
            Parameters = param;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0, "itemDef").attrInput;
            set => DoubleCheckAttr(0, "itemDef").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(1, "position").attrInput;
            set => DoubleCheckAttr(1, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Animate
        {
            get => DoubleCheckAttr(2, "bool").attrInput;
            set => DoubleCheckAttr(2, "bool").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Parameters
        {
            get => DoubleCheckAttr(3, "itemParam").attrInput;
            set => DoubleCheckAttr(3, "itemParam").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            string p = Macrolize(3);
            string[] pos = new string[] { null, null };
            if (string.IsNullOrEmpty(p)) p = "_";
            if (Macrolize(2) == "false")
            {
                yield return sp + "last = New(_editor_class[" + Macrolize(0) + "], " + Macrolize(1) + ", " + p + ")\n";
            }
            else
            {
                pos = Macrolize(1).Split(new string[] { ", ", "," }, StringSplitOptions.None);
                yield return sp + "local r2 = sqrt(ran:Float(1, 4)) * sqrt(0) * 5\n"
                            + sp + "local a = ran:Float(0, 360)\n"
                            + sp + "last = New(_editor_class[" + Macrolize(0) + $"], {pos[0]} + r2 * cos(a), {pos[1]} + r2 * sin(a), " + p + ")\n";
            }
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Create item " + NonMacrolize(0) + " at (" + NonMacrolize(1) + ") with parameter " + NonMacrolize(3);
        }

        public override object Clone()
        {
            var n = new CreateItem(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override MetaInfo GetReferredMeta()
        {
            AttrItem original = attributes[0];
            AbstractMetaData metaData = original.Parent.parentWorkSpace.Meta;
            return metaData.aggregatableMetas[(int)MetaType.Item]
                .FindOfName(original.Parent.NonMacrolize(0).Trim('\"')) as MetaInfo;
        }
    }
}
