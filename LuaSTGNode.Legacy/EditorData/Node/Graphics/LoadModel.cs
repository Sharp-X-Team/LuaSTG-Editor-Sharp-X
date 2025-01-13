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
    [Serializable, NodeIcon("loadmodel3d.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class LoadModel : TreeNode
    {
        [JsonConstructor]
        private LoadModel() : base() { }

        public LoadModel(DocumentData workSpaceData)
            : this(workSpaceData, "", "") { }

        public LoadModel(DocumentData workSpaceData, string path, string name)
            : base(workSpaceData)
        {
            Path = path;
            ResourceName = name;
        }

        [JsonIgnore, NodeAttribute]
        public string Path
        {
            get => DoubleCheckAttr(0, "modelFile", isDependency: true).attrInput;
            set => DoubleCheckAttr(0, "modelFile", isDependency: true).attrInput = value;
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
            yield return sp + $"LoadModel(\"model:\" .. \"{Lua.StringParser.ParseLua(NonMacrolize(1))}\", \"{sk}\")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Load model \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) + "\"";
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

        public override MetaInfo GetMeta()
        {
            return new ModelLoadMetaInfo(this);
        }

        public override object Clone()
        {
            LoadModel n = new(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
