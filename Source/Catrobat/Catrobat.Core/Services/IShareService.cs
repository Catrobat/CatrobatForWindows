﻿using System;
using System.Threading.Tasks;
using Catrobat.IDE.Core.CatrobatObjects;

namespace Catrobat.IDE.Core.Services
{
    public interface IShareService
    {
        //void ShareProjectWithMail(string projectName, string mailTo, string subject, string message);


        //// ### SkyDrive #############################################################################

        //void UploadProjectToSkydrive(object liveConnectSessionChangedEventArgs, ProjectDummyHeader project,
        //    Action success, Action error);

        Task ShareFile(string fileToShare);
    }
}
