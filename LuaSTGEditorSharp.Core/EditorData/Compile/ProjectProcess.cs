﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Compile;
using LuaSTGEditorSharp.EditorData.Exception;

namespace LuaSTGEditorSharp.EditorData.Compile
{
    /// <summary>
    /// The <see cref="CompileProcess"/> of luastg project file (.lstgproj).
    /// </summary>
    internal class ProjectProcess : CompileProcess
    {
        /// <summary>
        /// The child <see cref="PartialProjectProcess"/> of the process need to do.
        /// </summary>
        internal readonly List<PartialProjectProcess> fileProcess = [];

        /// <summary>
        /// Execute the <see cref="CompileProcess"/>.
        /// </summary>
        /// <param name="SCDebug">Whether SCDebug is switched on.</param>
        /// <param name="StageDebug">Whether Stage Debug is switched on.</param>
        public override void ExecuteProcess(bool SCDebug, bool StageDebug, IAppSettings appSettings)
        {
            GenerateCode(SCDebug, StageDebug);
            WriteRoot();

            //Gather file need to pack
            Dictionary<string, string> resNeedToPack = [];
            Dictionary<string, Tuple<string, string>> resPathToMD5 = [];

            if (appSettings.SaveResMeta)
            {
                if (File.Exists(projMetaPath) && File.Exists(targetZipPath))
                {
                    GatherResByResMeta(resNeedToPack, resPathToMD5);
                }
                else
                {
                    GatherResAndSaveMeta(resNeedToPack);
                }
            }
            else
            {
                GatherAllRes(resNeedToPack);
            }

            PackFileUsingInfo(appSettings, resNeedToPack, resPathToMD5, true);

            foreach (PartialProjectProcess process in fileProcess)
            {
                process.ProgressChanged += ProgressChangedEventHandler;
                process.ExecuteProcess(SCDebug, StageDebug, appSettings);
            }
        }
    }
}
