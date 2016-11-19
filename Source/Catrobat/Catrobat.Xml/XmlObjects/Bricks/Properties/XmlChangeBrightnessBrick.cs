﻿using System.Xml.Linq;
using Catrobat.IDE.Core.Xml.XmlObjects.Formulas;

namespace Catrobat.IDE.Core.Xml.XmlObjects.Bricks.Properties
{
    public partial class XmlChangeBrightnessBrick : XmlBrick
    {
        public XmlFormula ChangeBrightness { get; set; }

        public override bool Equals(System.Object obj)        {
            XmlChangeBrightnessBrick b = obj as XmlChangeBrightnessBrick;
            if ((object)b == null)
                return false;

            return this.Equals(b) && this.ChangeBrightness.Equals(b.ChangeBrightness);
        }

        public bool Equals(XmlChangeBrightnessBrick b)        {
            return this.Equals((XmlBrick)b) && this.ChangeBrightness.Equals(b.ChangeBrightness);
        }

        public override int GetHashCode()        {
            return base.GetHashCode() ^ ChangeBrightness.GetHashCode();
        }

        public XmlChangeBrightnessBrick() { }

        public XmlChangeBrightnessBrick(XElement xElement) : base(xElement) { }

        internal override void LoadFromXml(XElement xRoot)
        {
            if (xRoot != null)
            {
                ChangeBrightness = new XmlFormula(xRoot, XmlConstants.ChangeBrightness);
            }
        }

        internal override XElement CreateXml()
        {
            var xRoot = new XElement(XmlConstants.Brick);
            xRoot.SetAttributeValue(XmlConstants.Type, XmlConstants.XmlChangeBrightnessBrickType);

            var xElement = ChangeBrightness.CreateXml();
            xElement.SetAttributeValue(XmlConstants.Category, XmlConstants.ChangeBrightness);

            var xFormulalist = new XElement(XmlConstants.FormulaList);
            xFormulalist.Add(xElement);

            xRoot.Add(xFormulalist);

            return xRoot;
        }

        public override void LoadReference()
        {
            if (ChangeBrightness != null)
                ChangeBrightness.LoadReference();
        }
    }
}
