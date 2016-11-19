﻿using Catrobat.IDE.Core.Models;
using Catrobat.IDE.Core.Models.Bricks;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.Xml.XmlObjects.Bricks.ControlFlow;

namespace Catrobat.IDE.Core.XmlModelConvertion.Converters.Actions.Bricks
{
    public class BroadcastSendWaitBrickConverter : BrickConverterBase<XmlBroadcastWaitBrick, BroadcastSendBlockingBrick>
    {
        public BroadcastSendWaitBrickConverter() { }

        public override BroadcastSendBlockingBrick Convert1(XmlBroadcastWaitBrick o, XmlModelConvertContext c)
        {
            BroadcastMessage message = null;
            if (o.BroadcastMessage != null)
            {
                if (!c.BroadcastMessages.TryGetValue(o.BroadcastMessage, out message))
                {
                    message = new BroadcastMessage
                    {
                        Content = o.BroadcastMessage
                    };
                    c.BroadcastMessages.Add(o.BroadcastMessage, message);
                }
            }
            return new BroadcastSendBlockingBrick
            {
                Message = message
            };
        }

        public override XmlBroadcastWaitBrick Convert1(BroadcastSendBlockingBrick m, XmlModelConvertBackContext c)
        {
            return new XmlBroadcastWaitBrick
            {
                BroadcastMessage = m.Message == null ? null : m.Message.Content
            };
        }
    }
}
