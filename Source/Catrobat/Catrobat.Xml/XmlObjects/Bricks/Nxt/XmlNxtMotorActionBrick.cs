﻿using System.Xml.Linq;
using Catrobat.IDE.Core.Xml.XmlObjects.Formulas;

namespace Catrobat.IDE.Core.Xml.XmlObjects.Bricks.Nxt
{
    public partial class XmlNxtMotorActionBrick : XmlBrick
    {
        public string Motor { get; set; }

        public XmlFormula Speed { get; set; }

        public XmlNxtMotorActionBrick() {}

        public XmlNxtMotorActionBrick(XElement xElement) : base(xElement) {}

        internal override void LoadFromXml(XElement xRoot)
        {
            Motor = xRoot.Element("motor").Value;
            Speed = new XmlFormula(xRoot.Element("speed"));
        }

        internal override XElement CreateXml()
        {
            var xRoot = new XElement("brick");
            xRoot.SetAttributeValue("type", "legoNxtMotorActionBrick");

            xRoot.Add(new XElement("motor")
            {
                Value = Motor
            });

            var xVariable = new XElement("speed");
            xVariable.Add(Speed.CreateXml());
            xRoot.Add(xVariable);

            return xRoot;
        }

        public override void LoadReference()
        {
            if (Speed != null)
                Speed.LoadReference();
        }
    }
}
