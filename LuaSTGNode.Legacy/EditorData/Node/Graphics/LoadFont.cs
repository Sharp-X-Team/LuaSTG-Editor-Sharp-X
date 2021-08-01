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
    [Serializable, NodeIcon("loadfont.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0)]
    public class LoadFont : TreeNode
    {
        [JsonConstructor]
        public LoadFont() : base() { }

        public LoadFont(DocumentData workSpaceData) : this(workSpaceData, "", "", "false") { }
        public LoadFont(DocumentData workSpaceData, string path, string resName, string mipm) : base(workSpaceData) 
        {
            Path = path;
            ResourceName = resName;
            Mipmap = mipm;
            //Pngsel = pngs;
        }

        [JsonIgnore, NodeAttribute]
        public string Path
        {
            get => DoubleCheckAttr(0, "fontFile", isDependency: true).attrInput;
            set => DoubleCheckAttr(0, "fontFile", isDependency: true).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ResourceName
        {
            get => DoubleCheckAttr(1, name: "Font name").attrInput;
            set => DoubleCheckAttr(1, name: "Font name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Mipmap
        {
            get => DoubleCheckAttr(2, "bool", "Mipmap").attrInput;
            set => DoubleCheckAttr(2, "bool", "Mipmap").attrInput = value;
        }

        //[JsonIgnore, NodeAttribute]
        //public string Pngsel
        //{
        //    get => DoubleCheckAttr(3, "imageFile", "Font image").attrInput;
        //    set => DoubleCheckAttr(3, "imageFile", "Font image").attrInput = value;
        //}

        public override string ToString()
        {
            return "Load Font \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) + "\" with mipmap " + NonMacrolize(2) + "";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = "font" + "\\\\" + GetRawPath(0);
            string sp = Indent(spacing);
            //string sk2 = sk.Replace("fnt", "bmp");
            //string skpng = "_LoadImageFromFile(\'" + NonMacrolize(1) + "\',\'" + sk2 + "\', " + NonMacrolize(2) + ", 0, 0, false, 0)";
            yield return sp + "" + "" + "" +
                $"lstg.LoadFont(\'font:\'..\'{Lua.StringParser.ParseLua(NonMacrolize(1))}\',\'{sk}\'," + Macrolize(2) + ")\n";
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
            
            string sk = "font\\" + System.IO.Path.GetFileName(NonMacrolize(0));
            if (!parentWorkSpace.CompileProcess.resourceFilePath.ContainsKey(NonMacrolize(0)))
            {
                parentWorkSpace.CompileProcess.resourceFilePath.Add(sk, attributes[0].AttrInput);
            }
            
        }

        public override MetaInfo GetMeta()
        {
            return new FontLoadMetaInfo(this);
        }

        public override object Clone()
        {
            var n = new LoadFont(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}
