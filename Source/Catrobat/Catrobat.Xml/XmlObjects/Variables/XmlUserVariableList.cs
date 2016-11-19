﻿using System.Collections.Generic;
using System.Xml.Linq;

namespace Catrobat.IDE.Core.Xml.XmlObjects.Variables
{
    public class XmlUserVariableList : XmlObjectNode
    {
        public List<XmlUserVariable> UserVariables;

        public override bool Equals(System.Object obj)
        {
            XmlUserVariableList l = obj as XmlUserVariableList;
            if ((object)l == null)
            {
                return false;
            }

            return this.Equals(l);
        }

        public bool Equals(XmlUserVariableList l)
        {
            return this.UserVariables.Equals(l.UserVariables);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ UserVariables.GetHashCode();
        }

        public XmlUserVariableList() {UserVariables = new List<XmlUserVariable>();}

        public XmlUserVariableList(XElement xElement)
        {
            UserVariables = new List<XmlUserVariable>();
            LoadFromXml(xElement);
        }

        internal override void LoadFromXml(XElement xRoot)
        {
            if (xRoot == null)
                return;

            foreach (XElement element in xRoot.Elements())
            {
                UserVariables.Add(new XmlUserVariable(element));
            }
        }

        internal override XElement CreateXml()
        {
            var xRoot = new XElement(XmlConstants.List);

            foreach (XmlUserVariable userVariable in UserVariables)
            {
                xRoot.Add(userVariable.CreateXml());
            }

            return xRoot;
        }
    }
}
