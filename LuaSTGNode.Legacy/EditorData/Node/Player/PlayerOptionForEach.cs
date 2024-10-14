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
    [Serializable, NodeIcon("playeroptionforeach.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [IgnoreAttributesParityCheck]
    public class PlayerOptionForEach : TreeNode
    {
        [JsonConstructor]
        private PlayerOptionForEach() : base() { }

        public PlayerOptionForEach(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            yield return 
                sp + "for i = 1, 4 do\n" +
                sp + s1 + "if self.sp[i] and self.sp[i][3] > 0.5 then\n" +
                sp + s1 + s1 + "local option = { x = self.supportx + self.sp[i][1], y = self.supporty + self.sp[i][2]}\n";
            foreach (var a in base.ToLua(spacing + 2))
            {
                yield return a;
            }
            yield return
                sp + s1 + "end\n" +
                sp + "end\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Do action for every player option, using target \"option\"";
        }

        public override object Clone()
        {
            var n = new PlayerOptionForEach(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
