﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catrobat.IDE.Core.Models;
using Catrobat.IDE.Core.Xml;
using Catrobat.IDE.Core.Xml.XmlObjects;

namespace Catrobat.IDE.Core.Services
{
    public interface IContextService
    {
        Task<string> ConvertToValidFileName(string fileName);

        Task<string> FindUniqueName(string requestedName, List<string> nameList);

        Task<string> FindUniqueProgramName(string programName);
        Task<XmlProgramRenamerResult> RenameProgram(
            string programCodeFilePath, string newProgramName);

        Task<Program> LoadProgramByName(string programName);

        Task<XmlProgram> LoadXmlProgramByName(string programName);

        Task<Program> RestoreDefaultProgram(string programName);

        Task<Program> CreateEmptyProgram(string newProgramName);

        Task<string> CopyProgramPart1(string sourceProgramName);

        Task<Program> CopyProgramPart2(string sourceProgramName, string newProgramName);

        Task<Program> CopyProgram(string sourceProgramName, string newProgramName);

        Task StoreLocalSettings(LocalSettings localSettings);

        Task<LocalSettings> RestoreLocalSettings();

        Task CreateThumbnailsForNewProgram(string programName);

        void UpdateProgramHeader(XmlProgram program);
    }
}
