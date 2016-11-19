﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catrobat.IDE.Core.CatrobatObjects;
using Catrobat.IDE.Core.ExtensionMethods;
using Catrobat.IDE.Core.Models;
using Catrobat.IDE.Core.Services.Storage;
using Catrobat.IDE.Core.UI.PortableUI;
using Catrobat.IDE.Core.Xml;
using Catrobat.IDE.Core.Xml.VersionConverter;
using Catrobat.IDE.Core.Xml.XmlObjects;
using Catrobat.IDE.Core.XmlModelConvertion.Converters;
using Catrobat_Player.NativeComponent;

namespace Catrobat.IDE.Core.Services.Common
{
    public class ProgramValidationService : IProgramValidationService
    {
        public async Task<CheckProgramResult> CheckProgram(string pathToProgramDirectory)
        {
            var pathToProgramCodeFile = Path.Combine(pathToProgramDirectory, StorageConstants.ProgramCodePath);

            XmlProgram convertedProgram = null;
            var checkResult = new CheckProgramResult();
            PortableImage programScreenshot = null;
            string programName = null;
            string programCode = null;

            using (var storage = StorageSystem.GetStorage())
            {
                programScreenshot =
                    await storage.LoadImageAsync(Path.Combine(
                    pathToProgramDirectory,
                    StorageConstants.ProgramManualScreenshotPath)) ??
                    await storage.LoadImageAsync(Path.Combine(
                    pathToProgramDirectory,
                    StorageConstants.ProgramAutomaticScreenshotPath));

                if (!await storage.FileExistsAsync(pathToProgramCodeFile))
                {
                    checkResult.State = ProgramState.FilesMissing;
                    return checkResult;
                }
                programCode = await storage.ReadTextFileAsync(pathToProgramCodeFile);                
            }

            var converterResult = await CatrobatVersionConverter.
                ConvertToXmlVersion(programCode, XmlConstants.TargetIDEVersion);

            if (converterResult.Error != CatrobatVersionConverter.VersionConverterStatus.NoError)
            {
                switch (converterResult.Error)
                {
                    case CatrobatVersionConverter.VersionConverterStatus.VersionTooNew:
                        checkResult.State = ProgramState.VersionTooNew;
                        break;
                    case CatrobatVersionConverter.VersionConverterStatus.VersionTooOld:
                        checkResult.State = ProgramState.VersionTooOld;
                        break;
                    default:
                        checkResult.State = ProgramState.Damaged;
                        break;
                }
                return checkResult;
            }

            try
            {
                convertedProgram = new XmlProgram(converterResult.Xml);
                programName = convertedProgram.ProgramHeader.ProgramName;
            }
            catch (Exception)
            {
                checkResult.State = ProgramState.Damaged;
                checkResult.ProgramHeader = null;
                checkResult.Program = null;
                return checkResult;
            }

            try
            {
                ProgramConverter programConverter = new ProgramConverter();
                checkResult.Program = programConverter.Convert(convertedProgram);
                NativeWrapper.SetProject(convertedProgram);
            }
            catch (Exception)
            {
                checkResult.State = ProgramState.ErrorInThisApp;
                checkResult.ProgramHeader = null;
                checkResult.Program = null;
                return checkResult;
            }

            if(programName == null)
                programName = XmlProgramHelper.GetProgramName(converterResult.Xml);

            checkResult.ProgramHeader = new LocalProgramHeader
            {
                Screenshot = programScreenshot,
                ProjectName = programName,
            };

            checkResult.State = ProgramState.Valid;
            return checkResult;
        }
    }
}
