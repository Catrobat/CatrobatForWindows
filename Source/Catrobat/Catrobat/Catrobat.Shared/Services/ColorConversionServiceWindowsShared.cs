﻿using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.UI.PortableUI;

namespace Catrobat.IDE.WindowsShared.Services
{
    public class ColorConversionServiceWindowsShared : IColorConversionService
    {
        public object ConvertToLocalSolidColorBrush(PortableSolidColorBrush portableSolidColorBrush)
        {
            var b = portableSolidColorBrush;
            return new SolidColorBrush(new Color { A = b.A, R = b.R, G = b.G, B = b.B });
        }

        public PortableSolidColorBrush ConvertFromLocalSolidColorBrush(object localSolidColorBrush)
        {
            if (!(localSolidColorBrush is SolidColorBrush))
                throw new ArgumentException("localSolidColorBrush must be of type SolidColorBrush");

            var b = (SolidColorBrush)localSolidColorBrush;
            return new PortableSolidColorBrush { A = b.Color.A, R = b.Color.R, G = b.Color.G, B = b.Color.B };
        }


        public object ConvertToLocalSolidColor(PortableSolidColorBrush portableSolidColor)
        {
            throw new NotImplementedException();
        }
    }
}
