﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Catrobat.IDE.Core.Xml.XmlObjects.Formulas;
using Catrobat_Player.NativeComponent;

namespace Catrobat.IDE.Core.Xml.XmlObjects.Bricks.ControlFlow
{
    public partial class XmlRepeatBrick : XmlLoopBeginBrick, IRepeatBrick
    {
        #region NativeInterface
        IFormulaTree IRepeatBrick.TimesToRepeat
        {
            get
            {
                return TimesToRepeat == null ? null : TimesToRepeat.FormulaTree;
            }
            set { }
        }

        public IList<IBrick> Bricks
        {
            get
            {
                throw new NotImplementedException();
            }
            set { }
        }

        #endregion

        public XmlFormula TimesToRepeat { get; set; }

        public XmlRepeatBrick() {}

        public XmlRepeatBrick(XElement xElement) : base(xElement) {}

        #region equals_and_gethashcode
        public override bool Equals(System.Object obj)
        {
            XmlRepeatBrick b = obj as XmlRepeatBrick;
            if ((object)b == null)
                return false;

            return this.Equals(b) && this.TimesToRepeat.Equals(b.TimesToRepeat);
        }

        public bool Equals(XmlRepeatBrick b)
        {
            return this.Equals((XmlBrick)b) && this.TimesToRepeat.Equals(b.TimesToRepeat);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ TimesToRepeat.GetHashCode();
        }
        #endregion


        internal override void LoadFromXml(XElement xRoot)
        {
            if (xRoot != null)
            {
                TimesToRepeat = new XmlFormula(xRoot, XmlConstants.TimesToRepeat);
            }
        }

        internal override XElement CreateXml()
        {
            var xRoot = new XElement(XmlConstants.Brick);
            xRoot.SetAttributeValue(XmlConstants.Type, XmlConstants.XmlRepeatBrickType);
            base.CreateCommonXML(xRoot);

            var xElement = TimesToRepeat.CreateXml();
            xElement.SetAttributeValue(XmlConstants.Category, XmlConstants.TimesToRepeat);

            var xFormulalist = new XElement(XmlConstants.FormulaList);
            xFormulalist.Add(xElement);
            
            xRoot.Add(xFormulalist);

            return xRoot;
        }

        public override void LoadReference()
        {
            base.LoadReference();

            if (TimesToRepeat != null)
                TimesToRepeat.LoadReference();
        }
    }
}
