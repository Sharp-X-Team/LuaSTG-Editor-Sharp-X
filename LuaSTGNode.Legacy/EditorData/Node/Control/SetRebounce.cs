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

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("objectrebounce.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class SetRebounce : TreeNode
    {
        [JsonConstructor]
        private SetRebounce() : base() { }

        public SetRebounce(DocumentData workSpaceData)
            : this(workSpaceData, "_infinite") { }

        public SetRebounce(DocumentData workSpaceData, string valu)
            : base(workSpaceData)
        {
            Ppty = valu;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Ppty
        {
            get => DoubleCheckAttr(0, name: "Rebounce times").attrInput;
            set => DoubleCheckAttr(0, name: "Rebounce times").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            yield return sp + "lasttask=task.New(self,function()\n" +
                         sp + s1 + "local w = lstg.world\n" +
                         sp + s1 + "self.tbounce, self.tbouncemax = 0, " + Macrolize(0) + "\n" +
                         sp + s1 + "for _=1, _infinite do\n" +
                         sp + s1 + s1 + "if self.y > w.t or self.y < w.b then\n" +
                         sp + s1 + s1 + s1 + "self.vy = self.vy * -1\n" +
                         sp + s1 + s1 + s1 + "self.tbounce = self.tbounce + 1\n" +
                         sp + s1 + s1 + "end\n" +
                         sp + s1 + s1 + "if self.x > w.r or self.x < w.l then\n" +
                         sp + s1 + s1 + s1 + "self.vx = self.vx * -1\n" +
                         sp + s1 + s1 + s1 + "self.tbounce = self.tbounce + 1\n" +
                         sp + s1 + s1 + "end\n" +
                         sp + s1 + s1 + "if self.tbounce >= self.tbouncemax then break end\n" +
                         sp + s1 + s1 + "task._Wait(1)\n" +
                         sp + s1 + "end\n" +
                         sp + "end)\n"; 
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Rebounce self " + NonMacrolize(0) + " times";
        }

        public override object Clone()
        {
            var n = new SetRebounce(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
