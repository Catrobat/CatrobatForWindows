﻿using Catrobat.IDE.Core.Models.Bricks;
using Context = Catrobat.IDE.Core.Xml.Converter.XmlProjectConverter.ConvertContext;

// ReSharper disable once CheckNamespace
namespace Catrobat.IDE.Core.Xml.XmlObjects.Bricks.Sounds
{
    partial class XmlStopAllSoundsBrick
    {
        protected override Brick ToModel2(Context context)
        {
            return new StopSoundsBrick();
        }
    }
}