﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Laser
{
    [Serializable, NodeIcon("lasergrow.png")]
    [RequireAncestor(typeof(LaserAlikeTypes))]
    [RequireAncestor(typeof(TaskAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class LaserGrow : TreeNode
    {
        [JsonConstructor]
        private LaserGrow() : base() { }

        public LaserGrow(DocumentData workSpaceData)
            : this(workSpaceData, "self", "30", "true", "true")
        { }

        public LaserGrow(DocumentData workSpaceData, string target, string time, string se, string wait)
            : base(workSpaceData)
        {
            Target = target;
            Time = time;
            PlaySoundEffect = se;
            WaitInThisTask = wait;
            /*
            attributes.Add(new AttrItem("Target", target, this, "target"));
            attributes.Add(new AttrItem("Time", time, this, "time"));
            attributes.Add(new AttrItem("Play Sound Effect", se, this, "bool"));
            attributes.Add(new AttrItem("Wait in this Task", wait, this, "bool"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(1, "time").attrInput;
            set => DoubleCheckAttr(1, "time").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string PlaySoundEffect
        {
            get => DoubleCheckAttr(2, "bool", "Play Sound Effect").attrInput;
            set => DoubleCheckAttr(2, "bool", "Play Sound Effect").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string WaitInThisTask
        {
            get => DoubleCheckAttr(3, "bool", "Wait in this Task").attrInput;
            set => DoubleCheckAttr(3, "bool", "Wait in this Task").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "laser.grow(" + Macrolize(0) + ", " + Macrolize(1) + ", " + Macrolize(2)
                + ", " + Macrolize(3) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "\"" + NonMacrolize(0) + "\" grow in " + NonMacrolize(1) + " frame(s)"
                + (NonMacrolize(3) == "true" ? ", wait" : "")
                + (NonMacrolize(2) == "true" ? ", play sound effect" : "");
        }

        public override object Clone()
        {
            var n = new LaserGrow(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
