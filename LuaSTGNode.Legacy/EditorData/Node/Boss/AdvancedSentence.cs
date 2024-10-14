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

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("advancedsentence.png")]
    [RequireAncestor(typeof(Dialog))]
    [RequireAncestor(typeof(TaskAlikeTypes))]
    [LeafNode]
    [RCInvoke(2)]
    public class AdvancedSentence : TreeNode
    {
        [JsonConstructor]
        public AdvancedSentence() : base() { }

        public AdvancedSentence(DocumentData workSpaceData) 
            : this(workSpaceData, "\"img_void\"", "\"left\"", "", "600", "1, 1", "1", "1", "nil", "128", "nil", "230", "1", "false") { }

        public AdvancedSentence(DocumentData workSpaceData, string img, string lr, string txt, string time, string scale, string tpics, string nums, string pxs, string pys, string txs, string tys, string tns, string stays) 
            : base(workSpaceData)
        {
            Image = img;
            ImagePosition = lr;
            Text = txt;
            Time = time;
            Scale = scale;
            Tpic = tpics;
            Num = nums;
            Px = pxs;
            Py = pys;
            Tx = txs;
            Ty = tys;
            Tn = tns;
            Stay = stays;
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(0, "image").attrInput;
            set => DoubleCheckAttr(0, "image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ImagePosition
        {
            get => DoubleCheckAttr(1, "lrstr", "Image position").attrInput;
            set => DoubleCheckAttr(1, "lrstr", "Image position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Text
        {
            get => DoubleCheckAttr(2, "multilineText").attrInput;
            set => DoubleCheckAttr(2, "multilineText").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(3).attrInput;
            set => DoubleCheckAttr(3).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Scale
        {
            get => DoubleCheckAttr(4, "scale").attrInput;
            set => DoubleCheckAttr(4, "scale").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Tpic
        {
            get => DoubleCheckAttr(5, "bubble_style", "Balloon type (tpic)").attrInput;
            set => DoubleCheckAttr(5, "bubble_style", "Balloon type (tpic)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Num
        {
            get => DoubleCheckAttr(6, name: "Master Char (num)").attrInput;
            set => DoubleCheckAttr(6, name: "Master Char (num)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Px
        {
            get => DoubleCheckAttr(7, name: "Image X offset (Px)").attrInput;
            set => DoubleCheckAttr(7, name: "Image X offset (Px)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Py
        {
            get => DoubleCheckAttr(8, name: "Image Y offset (Py)").attrInput;
            set => DoubleCheckAttr(8, name: "Image Y offset (Py)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Tx
        {
            get => DoubleCheckAttr(9, name: "Balloon X offset (Tx)").attrInput;
            set => DoubleCheckAttr(9, name: "Balloon X offset (Tx)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Ty
        {
            get => DoubleCheckAttr(10, name: "Balloon Y offset (Ty)").attrInput;
            set => DoubleCheckAttr(10, name: "Balloon Y offset (Ty)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Tn
        {
            get => DoubleCheckAttr(11, name: "Remain for (x) sentences (tn)").attrInput;
            set => DoubleCheckAttr(11, name: "Remain for (x) sentences (tn)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Stay
        {
            get => DoubleCheckAttr(12, "bool", "Stay").attrInput;
            set => DoubleCheckAttr(12, "bool", "Stay").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string attr3 = Macrolize(3);
            attr3 = string.IsNullOrEmpty(attr3) ? "nil" : attr3;
            string attr4 = Macrolize(4);
            attr4 = string.IsNullOrEmpty(attr4) ? "nil,nil" : attr4;
            string attr7 = Macrolize(7);
            attr7 = string.IsNullOrEmpty(attr7) ? "nil" : attr7;
            string attr9 = Macrolize(9);
            attr9 = string.IsNullOrEmpty(attr9) ? "nil" : attr9;
            yield return sp + "boss.dialog.sentence(self, " + Macrolize(0) + ", " + Macrolize(1) + ", [===[" 
                + NonMacrolize(2) + "]===], " + attr3 + ", " + attr4 + ", " + Macrolize(5) + ", " + Macrolize(6) +
                ", " + attr7 + ", " + Macrolize(8) + ", " + attr9 + ", " + Macrolize(10) + ", " + Macrolize(11) +
                ", " + Macrolize(12) + ")\n";
        }

        public override string ToString()
        {
            return NonMacrolize(0) + " " + NonMacrolize(1) + " :\n" + NonMacrolize(2) + "\nwait "
                + (string.IsNullOrEmpty(NonMacrolize(3)) ? "default" : NonMacrolize(3)) + " frame(s), image scale is (" + NonMacrolize(4) + ")\n" +
                "Balloon type is " + NonMacrolize(5) + ", num is " + NonMacrolize(6) + ", px, py = (" + NonMacrolize(7) + "," + NonMacrolize(8) + ")\n" +
                "tx, ty = (" + NonMacrolize(9) + "," + NonMacrolize(10) + "), tn is " + NonMacrolize(11) + ", stay is " + NonMacrolize(12) + "";
        }

        public override object Clone()
        {
            var n = new AdvancedSentence(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            string s = Macrolize(2);
            int i = 1;
            foreach (char c in s)
            {
                if (c == '\n')
                    i++;
            }
            yield return new Tuple<int, TreeNode>(i, this);
        }
    }
}
