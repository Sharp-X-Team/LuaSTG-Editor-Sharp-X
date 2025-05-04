using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Document
{
    public class DocumentCollection : ObservableCollection<DocumentData>
    {
        public int MaxHash { get; private set; } = 0;
        public static ILogger Logger { get; private set; } = EditorLogging.ForContext("DocumentCollection");

        public void AddAndAllocHash(DocumentData a)
        {
            base.Add(a);
            a.parent = this;
            MaxHash += 1;
            Logger.Information($"Document {a.DocName} added to collection. New MaxHash={MaxHash}.");
        }
    }
}
