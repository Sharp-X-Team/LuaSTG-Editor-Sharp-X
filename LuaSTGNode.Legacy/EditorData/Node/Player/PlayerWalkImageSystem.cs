using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Message;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("playerwalkimg.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(3)]
    public class SetPlayerWalkImageSystem : TreeNode
    {
        [JsonConstructor]
        private SetPlayerWalkImageSystem() : base() { }

        public SetPlayerWalkImageSystem(DocumentData workSpaceData)
            : this(workSpaceData, "", "", "0.5,0.5", "true") { }

        public SetPlayerWalkImageSystem(DocumentData workSpaceData, string imgpath, string resn, string collisize, string defsize) : base(workSpaceData)
        {
            ImagePath = imgpath;
            Resname = resn;
            CollisionSize = collisize;
            DefSize = defsize;
        }

        [JsonIgnore, NodeAttribute]
        public string ImagePath
        {
            get => DoubleCheckAttr(0, "imageFile", "Image path", isDependency: true).attrInput;
            set => DoubleCheckAttr(0, "imageFile", "Image path", isDependency: true).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Resname
        {
            get => DoubleCheckAttr(1, name: "Resource name").attrInput;
            set => DoubleCheckAttr(1, name: "Resource name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string CollisionSize
        {
            get => DoubleCheckAttr(2, "size", "Hitbox size").attrInput;
            set => DoubleCheckAttr(2, "size", "Hitbox size").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string DefSize
        {
            get => DoubleCheckAttr(3, "bool", "Is Default Rect").attrInput;
            set => DoubleCheckAttr(3, "bool", "Is Default Rect").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = GetPath(0);
            string sp = Indent(spacing);
            string load_method = Macrolize(3) == "false" ? "_LoadImageGroupFromFile(\'" + Macrolize(1) + "\', \'" + sk + "\', false, 8, 3, 0.5, 0.5, false)\n" :
                                                           "LoadTexture('" + Macrolize(1) + "','" + sk + "')\n" +
                                                           sp + "LoadImageGroup('" + Macrolize(1) + "','" + Macrolize(1) + "',0,0,32,48,8,3," + Macrolize(2) + ")\n";
            yield return sp + load_method +
                         sp + "self.imgs = {}\n" +
                         sp + "self.A, self.B = " + Macrolize(2) + "\n" +
                         sp + "for i = 1, 24 do self.imgs[i]='" + Macrolize(1) + "'..i end\n";
            //yield return sp + "LoadTexture('" + Macrolize(1) + "','" + sk + "')\n" +
                         //sp + "LoadImageGroup('" + Macrolize(1) + "','" + Macrolize(1) + "',0,0,32,48,8,3," + Macrolize(2) + ")\n" +
                         //sp + "self.imgs = {}\n" +
                         //sp + "self.A, self.B = " + Macrolize(2) + "\n" +
                         //sp + "for i = 1, 24 do self.imgs[i]='" + Macrolize(1) + "'..i end\n";
        }

        public override string ToString()
        {
            return "Set walk image of current player from \"" + NonMacrolize(0) + "\" and its behaviour to that of player";
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
            string sk = parentWorkSpace.CompileProcess.archiveSpace + Path.GetFileName(NonMacrolize(0));
            if (!string.IsNullOrEmpty(NonMacrolize(0))
                && !parentWorkSpace.CompileProcess.resourceFilePath.ContainsKey(sk))
                parentWorkSpace.CompileProcess.resourceFilePath.Add(sk, NonMacrolize(0));
        }

        public override object Clone()
        {
            var n = new SetPlayerWalkImageSystem(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(2, this);
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            return messages;
        }
    }
}
