﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Catrobat.Player.StandAlone.DataTypes
{
    [XmlInclude(typeof(WhenScript))]
    public abstract class BaseScript
    {
    }

}
