using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Graphics
{
    [Serializable, NodeIcon("loadfontimage.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(3)]
    public class LoadFontImage : TreeNode
    {
        [JsonConstructor]
        private LoadFontImage() : base() { }

        public LoadFontImage(DocumentData workSpaceData)
            : this(workSpaceData, "", "") { }

        public LoadFontImage(DocumentData workSpaceData, string path, string name)
            : base(workSpaceData)
        {
            Path = path;
            ResourceName = name;
            /*
            attributes.Add(new DependencyAttrItem("Path", path, this, "imageFile"));
            attributes.Add(new AttrItem("Resource name", name, this));
            attributes.Add(new AttrItem("Mipmap", mipmap, this, "bool"));
            attributes.Add(new AttrItem("Collision size", collis, this, "size"));
            attributes.Add(new AttrItem("Rectangle collision", rect, this, "bool"));
            attributes.Add(new AttrItem("Cut edge", edge, this));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Path
        {
            get => DoubleCheckAttr(0, "imageFile", isDependency: true).attrInput;
            set => DoubleCheckAttr(0, "imageFile", isDependency: true).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ResourceName
        {
            get => DoubleCheckAttr(1, name: "Resource name").attrInput;
            set => DoubleCheckAttr(1, name: "Resource name").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = GetPath(0);
            string sp = Indent(spacing);
            // string thing = "_LoadImageFromFile(\'image:\'..\'" + Lua.StringParser.ParseLua(NonMacrolize(1))
                //+ "\',\'" + sk
                //+ "\'," + Macrolize(2) + "," + Macrolize(3) + "," + Macrolize(4) + "," + Macrolize(5) + ")\n";
            yield return sp + "--- Load Font Image \"" + Macrolize(1) + "\"\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Load font image \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) + "\"";
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            if (relatedAttrItem.AttrInput != args.originalValue) 
            {
                attributes[1].AttrInput = System.IO.Path.GetFileNameWithoutExtension(attributes[0].AttrInput);
            }
        }

        protected override void AddCompileSettings()
        {
            string sk = "font\\" + System.IO.Path.GetFileName(NonMacrolize(0));
            if (!parentWorkSpace.CompileProcess.resourceFilePath.ContainsKey(NonMacrolize(0)))
            {
                parentWorkSpace.CompileProcess.resourceFilePath.Add(sk, attributes[0].AttrInput);
            }
        }

        public override object Clone()
        {
            var n = new LoadFontImage(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
