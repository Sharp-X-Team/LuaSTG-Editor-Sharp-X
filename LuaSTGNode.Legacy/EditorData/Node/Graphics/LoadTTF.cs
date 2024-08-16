using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Graphics
{
    [Serializable, NodeIcon("loadttf.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0)]
    public class LoadTTF : TreeNode
    {
        [JsonConstructor]
        public LoadTTF() : base() { }

        public LoadTTF(DocumentData workSpaceData) : this(workSpaceData, "", "", "24, 24") { }
        public LoadTTF(DocumentData workSpaceData, string path, string resName, string wihe) : base(workSpaceData) 
        {
            Path = path;
            ResourceName = resName;
            WiHe = wihe;
            //Pngsel = pngs;
        }

        [JsonIgnore, NodeAttribute]
        public string Path
        {
            get => DoubleCheckAttr(0, "ttfFile", isDependency: true).attrInput;
            set => DoubleCheckAttr(0, "ttfFile", isDependency: true).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ResourceName
        {
            get => DoubleCheckAttr(1, name: "Font name").attrInput;
            set => DoubleCheckAttr(1, name: "Font name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string WiHe
        {
            get => DoubleCheckAttr(2, name: "Width, Height").attrInput;
            set => DoubleCheckAttr(2, name: "Width, Height").attrInput = value;
        }

        //[JsonIgnore, NodeAttribute]
        //public string Pngsel
        //{
        //    get => DoubleCheckAttr(3, "imageFile", "Font image").attrInput;
        //    set => DoubleCheckAttr(3, "imageFile", "Font image").attrInput = value;
        //}

        public override string ToString()
        {
            return "Load TTF \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) + "\" with width, height = (" + NonMacrolize(2) + ")";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = "font" + "/" + GetRawPath(0);
            string sp = Indent(spacing);
            //string sk2 = sk.Replace("fnt", "bmp");
            //string skpng = "_LoadImageFromFile(\'" + NonMacrolize(1) + "\',\'" + sk2 + "\', " + NonMacrolize(2) + ", 0, 0, false, 0)";
            yield return sp + "" + "" + "" +
                $"lstg.LoadTTF(\'ttf:\'..\'{Lua.StringParser.ParseLua(NonMacrolize(1))}\',\'{sk}\'," + Macrolize(2) + ")\n";
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
            //string skc = parentWorkSpace.CompileProcess.archiveSpace + System.IO.Path.GetFileName(NonMacrolize(3));
            //if (!parentWorkSpace.CompileProcess.resourceFilePath.ContainsKey(NonMacrolize(3)))
            //{
            //    parentWorkSpace.CompileProcess.resourceFilePath.Add(skc, attributes[0].AttrInput);
            //}
            
            string sk = "font/" + System.IO.Path.GetFileName(NonMacrolize(0));
            if (!parentWorkSpace.CompileProcess.resourceFilePath.ContainsKey(NonMacrolize(0)))
            {
                parentWorkSpace.CompileProcess.resourceFilePath.Add(sk, attributes[0].AttrInput);
            }
            
        }

        public override MetaInfo GetMeta()
        {
            return new TTFLoadMetaInfo(this);
        }

        public override object Clone()
        {
            var n = new LoadTTF(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}
