﻿using Catrobat.IDE.Core.ExtensionMethods;
using Catrobat.IDE.Core.Xml.XmlObjects.Bricks;
using Catrobat.IDE.Core.Xml.XmlObjects.Bricks.ControlFlow;
using Context = Catrobat.IDE.Core.Xml.Converter.XmlProgramConverter.ConvertBackContext;

// ReSharper disable once CheckNamespace
namespace Catrobat.IDE.Core.Models.Bricks
{
    partial class BroadcastBrick
    {
    }

    #region Implementations

    partial class BroadcastSendBrick
    {
        protected internal override XmlBrick ToXmlObject2(Context context)
        {
            return new XmlBroadcastBrick
            {
                BroadcastMessage = Message == null ? string.Empty : Message.ToXmlObject()
            };
        }
    }

    partial class BroadcastSendBlockingBrick
    {
        protected internal override XmlBrick ToXmlObject2(Context context)
        {
            return new XmlBroadcastWaitBrick
            {
                BroadcastMessage = Message == null ? string.Empty : Message.ToXmlObject()
            };
        }
    }

    #endregion
}