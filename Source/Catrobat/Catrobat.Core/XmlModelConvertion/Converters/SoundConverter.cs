﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catrobat.IDE.Core.Models;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.Xml.XmlObjects;

namespace Catrobat.IDE.Core.XmlModelConvertion.Converters
{
    public class SoundConverter : XmlModelConverter<XmlSound, Sound>
    {
        public SoundConverter() { }

        public override Sound Convert(XmlSound o, XmlModelConvertContext c)
        {
            return new Sound
            {
                Name = o.Name,
                FileName = o.FileName
            };
        }

        public override XmlSound Convert(Sound m, XmlModelConvertBackContext c)
        {
            return new XmlSound
            {
                Name = m.Name,
                FileName = m.FileName
            };
        }
    }
}
