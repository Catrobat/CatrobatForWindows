﻿using System.Xml.Linq;
using Catrobat.IDE.Core.Utilities.Helpers;

namespace Catrobat.IDE.Core.Xml.XmlObjects.Bricks.ControlFlow
{
    public class XmlLoopEndBrickReference : XmlObjectNode
    {
        private string _reference;

        //protected string _classField;
        //public string Class
        //{
        //    get { return _classField; }
        //    set
        //    {
        //        if (_classField == value)
        //        {
        //            return;
        //        }

        //        _classField = value;
        //        RaisePropertyChanged();
        //    }
        //}

        public XmlLoopEndBrick LoopEndBrick { get; set; }

        public XmlLoopEndBrickReference()
        {
        }

        public XmlLoopEndBrickReference(XElement xElement)
        {
            LoadFromXml(xElement);
        }

        internal override void LoadFromXml(XElement xRoot)
        {
            //_classField = xRoot.Attribute("class").Value;
            _reference = xRoot.Attribute("reference").Value;
        }

        internal override XElement CreateXml()
        {
            var xRoot = new XElement("brick");
            xRoot.SetAttributeValue("type", "loopEndBrick");
            xRoot.Add(new XAttribute("reference", ReferenceHelper.GetReferenceString(this)));

            return xRoot;
        }

        public override void LoadReference()
        {
            if(LoopEndBrick == null)
                LoopEndBrick = ReferenceHelper.GetReferenceObject(this, _reference) as XmlLoopEndBrick;
            if (string.IsNullOrEmpty(_reference))
                _reference = ReferenceHelper.GetReferenceString(this);
        }
    }
}
