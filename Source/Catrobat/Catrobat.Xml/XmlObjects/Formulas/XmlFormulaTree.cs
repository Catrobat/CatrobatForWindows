﻿using Catrobat_Player.NativeComponent;
using System.Xml.Linq;
using System;

namespace Catrobat.IDE.Core.Xml.XmlObjects.Formulas
{
    public partial class XmlFormulaTree : XmlObjectNode, IFormulaTree
    {
        #region NativeInterface
        IFormulaTree IFormulaTree.LeftChild
        {
            get { return LeftChild; }
            set { }
        }

        IFormulaTree IFormulaTree.RightChild
        {
            get { return RightChild; }
            set { }
        }

        public string VariableType { get; set; }

        public string VariableValue { get; set; }

        #endregion

        public XmlFormulaTree LeftChild { get; set; }

        public XmlFormulaTree RightChild { get; set; }

        public override bool Equals(System.Object obj)
        {
            XmlFormulaTree t = obj as XmlFormulaTree;
            if ((object)t == null)
            {
                return false;
            }

            return this.Equals(t);
            }

        public bool Equals(XmlFormulaTree t)
        {
            bool type = this.VariableType.Equals(t.VariableType);
            bool value = this.VariableValue.Equals(t.VariableValue);
            bool left = false;
            if ((this.LeftChild == null) && (t.LeftChild == null))
                left = true;
            else if ((this.LeftChild != null) && (t.LeftChild != null))
                left = this.LeftChild.Equals(t.LeftChild);
            else left = false;

            bool right = false;
            if ((this.RightChild == null) && (t.RightChild == null))
                right = true;
            else if ((this.RightChild != null) && (t.RightChild != null))
                right = this.RightChild.Equals(t.RightChild);
            else right = false;

            return type && value && left && right;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ LeftChild.GetHashCode() ^ RightChild.GetHashCode();
        }

        public XmlFormulaTree()
        {
        }

        public XmlFormulaTree(XElement xElement)
        {
            LoadFromXml(xElement);
        }

        internal override void LoadFromXml(XElement xRoot)
        {
            if(xRoot.Element(XmlConstants.LeftChild) != null)
                LeftChild = new XmlFormulaTree(xRoot.Element(XmlConstants.LeftChild));
            if (xRoot.Element(XmlConstants.RightChild) != null)
                RightChild = new XmlFormulaTree(xRoot.Element(XmlConstants.RightChild));

            if (xRoot.Element(XmlConstants.Type) != null)
            VariableType = xRoot.Element(XmlConstants.Type).Value;
            if (xRoot.Element(XmlConstants.Value) != null)
            VariableValue = xRoot.Element(XmlConstants.Value).Value;
        }

        internal override XElement CreateXml()
        {
            var xRoot = new XElement(XmlConstants.Formula);

            if(LeftChild != null)
                xRoot.Add(LeftChild.CreateXML(XmlConstants.LeftChild));
            if(RightChild != null)
                xRoot.Add(RightChild.CreateXML(XmlConstants.RightChild));
            if (VariableType != null)
            {
                var variableTypeElement = new XElement(XmlConstants.Type);
                variableTypeElement.Value = VariableType;
                xRoot.Add(variableTypeElement);
            }
            if(VariableValue != null)
            {
                var variableValueElement = new XElement(XmlConstants.Value);
                variableValueElement.Value = VariableValue;
                xRoot.Add(variableValueElement);
            }
                

            return xRoot;
        }

        internal XElement CreateXML(string childName)
        {
            var xRoot = new XElement(childName);

            if (LeftChild != null)
                xRoot.Add(LeftChild.CreateXML(XmlConstants.LeftChild));
            if (RightChild != null)
                xRoot.Add(RightChild.CreateXML(XmlConstants.RightChild));
            if (VariableType != null)
                xRoot.Add(new XElement(XmlConstants.Type, VariableType));
            if (VariableValue != null)
                xRoot.Add(new XElement(XmlConstants.Value, VariableValue));

            return xRoot;
        }
    }
}
