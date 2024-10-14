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
    [Serializable, NodeIcon("loadtexture.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(3)]
    public class LoadTexture : TreeNode
    {
        [JsonConstructor]
        private LoadTexture() : base() { }

        public LoadTexture(DocumentData workSpaceData)
            : this(workSpaceData, "", "", "true") { }

        public LoadTexture(DocumentData workSpaceData, string path, string name, string mipmap)
            : base(workSpaceData)
        {
            Path = path;
            ResourceName = name;
            Mipmap = mipmap;
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

        [JsonIgnore, NodeAttribute]
        public string Mipmap
        {
            get => DoubleCheckAttr(2, "bool").attrInput;
            set => DoubleCheckAttr(2, "bool").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = GetPath(0);
            string sp = Indent(spacing);
            yield return sp + "LoadTexture(\'texture:\'..\'" + Lua.StringParser.ParseLua(NonMacrolize(1))
                + "\', \'" + sk
                + "\', " + Macrolize(2) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Load texture \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) + "\"";
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
            string sk = parentWorkSpace.CompileProcess.archiveSpace + System.IO.Path.GetFileName(NonMacrolize(0));
            if (!parentWorkSpace.CompileProcess.resourceFilePath.ContainsKey(sk))
            {
                parentWorkSpace.CompileProcess.resourceFilePath.Add(sk, NonMacrolize(0));
            }
        }

        //public override MetaInfo GetMeta()
        //{
        //    return new ImageLoadMetaInfo(this);
       // }

        public override object Clone()
        {
            var n = new LoadTexture(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
