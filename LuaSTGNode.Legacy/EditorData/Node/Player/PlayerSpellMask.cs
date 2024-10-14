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

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("playerspellmask.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class PlayerSpellMask : TreeNode
    {
        [JsonConstructor]
        private PlayerSpellMask() : base() { }

        public PlayerSpellMask(DocumentData workSpaceData)
            : this(workSpaceData, "255, 255, 255", "30", "60", "30") { }

        public PlayerSpellMask(DocumentData workSpaceData, string col, string starttime, string staytime, string fadetime)
            : base(workSpaceData)
        {
            Color = col;
            StartTime = starttime;
            StayTime = staytime;
            FadeTime = fadetime;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Color
        {
            get => DoubleCheckAttr(0, "RGB").attrInput;
            set => DoubleCheckAttr(0, "RGB").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string StartTime
        {
            get => DoubleCheckAttr(1, name: "Start Time (frames)").attrInput;
            set => DoubleCheckAttr(1, name: "Start Time (frames)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string StayTime
        {
            get => DoubleCheckAttr(2, name: "Stay Time (frames)").attrInput;
            set => DoubleCheckAttr(2, name: "Stay Time (frames)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string FadeTime
        {
            get => DoubleCheckAttr(3, name: "Fade Time (frames)").attrInput;
            set => DoubleCheckAttr(3, name: "Fade Time (frames)").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "New(player_spell_mask, " + Macrolize(0) + ", " + Macrolize(1) + ", " + Macrolize(2) + ", " + Macrolize(3) + ")\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Create player spell mask with colors (" + NonMacrolize(0) + "), with start time \"" +
                    NonMacrolize(1) + "\", stay time \"" + NonMacrolize(2) + "\", fade time \"" + NonMacrolize(3) + "\"";
        }

        public override object Clone()
        {
            var n = new PlayerSpellMask(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
