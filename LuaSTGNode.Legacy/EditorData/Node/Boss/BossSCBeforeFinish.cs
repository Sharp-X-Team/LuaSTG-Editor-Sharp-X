﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("taskcallback.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(BossSpellCard)), Uniqueness]
    public class BossSCBeforeFinish : TreeNode
    {
        [JsonConstructor]
        private BossSCBeforeFinish() : base() { }

        public BossSCBeforeFinish(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "function _tmp_sc:beforedel()\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Task before finish";
        }

        public override object Clone()
        {
            var n = new BossSCBeforeFinish(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
