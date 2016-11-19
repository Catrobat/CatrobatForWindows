﻿using Catrobat.IDE.Core.Models;
using Catrobat.IDE.Core.Models.Bricks;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.Xml.XmlObjects.Bricks.ControlFlow;

namespace Catrobat.IDE.Core.XmlModelConvertion.Converters.Actions.Bricks
{
    public class ForeverEndBrickConverter : BrickConverterBase<XmlForeverLoopEndBrick, EndForeverBrick>
    {
        public override EndForeverBrick Convert1(XmlForeverLoopEndBrick o, XmlModelConvertContext c)
        {
            var foreverBrickConverter = new ForeverBrickConverter();

            var result = new EndForeverBrick();
            c.Bricks[o] = result;
            return result;
        }

        public override XmlForeverLoopEndBrick Convert1(EndForeverBrick m, XmlModelConvertBackContext c)
        {
            var foreverBrickConverter = new ForeverBrickConverter();

            var result = new XmlForeverLoopEndBrick();
            c.Bricks[m] = result;
            return result;
        }
    }
}
