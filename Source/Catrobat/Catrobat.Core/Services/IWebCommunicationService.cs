﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Catrobat.IDE.Core.CatrobatObjects;
using Catrobat.IDE.Core.Xml.VersionConverter;
using Catrobat.IDE.Core.Utilities.JSON;
using Catrobat.IDE.Core.Services.Common;
using Catrobat.IDE.Core.Utilities;


namespace Catrobat.IDE.Core.Services
{
    public delegate void DownloadProgressUpdatedEventHandler(object sender, ProgressEventArgs e);

    public interface IWebCommunicationService
    {
        event DownloadProgressUpdatedEventHandler DownloadProgressChanged;
        
        Task<List<OnlineProgramHeader>> LoadOnlineProgramsAsync(string filterText, int offset, int count, CancellationToken taskCancellationToken);

        Task<List<OnlineProgramHeader>> LoadOnlinePrograms(string category, int offset, int count, CancellationToken token, string additionalSearchText = null);

        Task<Stream> DownloadAsync(string downloadUrl, string programName, CancellationToken taskCancellationToken);

        Task DownloadAsyncAlternativ(string downloadUrl, string programName);

        Task<JSONStatusResponse> CheckTokenAsync(string username, string token, string language = "en");

        Task<JSONStatusResponse> LoginOrRegisterAsync(string username, string password, string userEmail, string language = "en", string country = "AT");

        Task<JSONStatusResponse> UploadProgramAsync(string programTitle, string username, string token, CancellationToken taskCancellationToken, string language = "en");

        Task<JSONStatusResponse> ReportAsInappropriateAsync(string programId, string flagReason, string language = "en");

        Task<JSONStatusResponse> RecoverPasswordAsync(string recoveryUserData, string language = "en");

        void SetRecoveryHash(string recoveryHash);

        Task<JSONStatusResponse> ChangePasswordAsync(string newPassword, string newPasswortRepeated, string language = "en");

        bool NoUploadsPending();

        DateTime ConvertUnixTimeStamp(double timestamp);
    }
}
