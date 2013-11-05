﻿using System;
using Windows.UI.Xaml;
using Catrobat.IDE.Core.CatrobatObjects;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.ViewModel.Main;

namespace Catrobat.IDE.Store.Services
{
    public class PlayerLauncherServiceStore :IPlayerLauncherService
    {
        public void LaunchPlayer(Project project)
        {
            ServiceLocator.NavigationService.NavigateTo<PlayerLauncherViewModel>();
            //var navigationUri = "/Views/Main/PlayerLauncherView.xaml?ProjectName=" + project.ProjectHeader.ProgramName;
            //((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(new Uri(navigationUri, UriKind.Relative));
        }
    }
}
