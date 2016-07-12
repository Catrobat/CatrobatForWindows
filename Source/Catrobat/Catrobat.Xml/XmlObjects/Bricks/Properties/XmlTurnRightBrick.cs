﻿using System;
using System.Xml.Linq;
using Catrobat.IDE.Core.Xml.XmlObjects.Formulas;
using Catrobat_Player.NativeComponent;

namespace Catrobat.IDE.Core.Xml.XmlObjects.Bricks.Properties
{
    public partial class XmlTurnRightBrick : XmlBrick, ITurnRightBrick
    {
        #region NativeInterface
        public IFormulaTree Rotation
        {
            get
            {
                return Degrees == null ? null : Degrees.FormulaTree;
            }
            set { }
        }

        #endregion

        public XmlFormula Degrees { get; set; }

        public XmlTurnRightBrick() { }

        public XmlTurnRightBrick(XElement xElement) : base(xElement) { }

        #region equals_and_gethashcode
        public override bool Equals(System.Object obj)
        {
            XmlTurnRightBrick b = obj as XmlTurnRightBrick;
            if ((object)b == null)
                return false;

            return this.Equals(b) && this.Degrees.Equals(b.Degrees);
        }

        public bool Equals(XmlTurnRightBrick b)
        {
            return this.Equals((XmlBrick)b) && this.Degrees.Equals(b.Degrees);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Degrees.GetHashCode();
        }
        #endregion


        internal override void LoadFromXml(XElement xRoot)
        {
            if (xRoot != null)
            {
                Degrees = new XmlFormula(xRoot, XmlConstants.TurnRightDegrees);
            }
        }

        internal override XElement CreateXml()
        {
            var xRoot = new XElement(XmlConstants.Brick);
            xRoot.SetAttributeValue(XmlConstants.Type, XmlConstants.XmlTurnRightBrickType);

            var xElement = Degrees.CreateXml();
            xElement.SetAttributeValue(XmlConstants.Category, XmlConstants.TurnRightDegrees);

            var xFormulalist = new XElement(XmlConstants.FormulaList);
            xFormulalist.Add(xElement);

            xRoot.Add(xFormulalist);

            return xRoot;
        }

        public override void LoadReference()
        {
            if (Degrees != null)
                Degrees.LoadReference();
        }
    }
}
