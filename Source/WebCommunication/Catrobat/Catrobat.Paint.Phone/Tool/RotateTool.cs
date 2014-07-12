﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Catrobat.Paint.Phone.Command;
using ImageTools;

namespace Catrobat.Paint.Phone.Tool
{
    class RotateTool : ToolBase
    {
        private int _angle;
        private RotateTransform _rotateTransform;

        public RotateTool()
        {
            this.ToolType = ToolType.Rotate;
            _angle = 0;
            _rotateTransform = new RotateTransform();
            //PocketPaintApplication.GetInstance().PaintingAreaContentPanelGrid.RenderTransform = _rotateTransform;

        }

        public override void HandleDown(object arg)
        {
           
        }

        public override void HandleMove(object arg)
        {
            
        }

        public override void HandleUp(object arg)
        {
          

        }

        public override void Draw(object o)
        {
            
        }

        public void RotateLeft()
        {
            var rotateTransform = new RotateTransform();
            if (_angle == 0)
            {
                _angle = 270;
            }
            else
            {
                _angle -= 90;
            }
            rotateTransform.Angle = _angle;
            rotateTransform.CenterX = 225;
            rotateTransform.CenterY = 295;
            //PocketPaintApplication.GetInstance().PaintingAreaCanvas.RenderTransform = rotateTransform;
            PocketPaintApplication.GetInstance().PaintingAreaContentPanelGrid.RenderTransform = rotateTransform;
            PocketPaintApplication.GetInstance().PaintingAreaContentPanelGrid.UpdateLayout();
            PocketPaintApplication.GetInstance().PaintingAreaContentPanelGrid.InvalidateArrange();
            PocketPaintApplication.GetInstance().PaintingAreaContentPanelGrid.InvalidateMeasure();

            //CommandManager.GetInstance().CommitCommand(new RotateCommand(RotateCommand.Direction.Left, _angle));

        }

        public void RotateRight()
        {
            var rotateTransform = new RotateTransform();
            _angle += 90;
            rotateTransform.Angle = _angle;
            rotateTransform.CenterX = 225;
            rotateTransform.CenterY = 295;
            //PocketPaintApplication.GetInstance().PaintingAreaCanvas.RenderTransform = rotateTransform;
            PocketPaintApplication.GetInstance().PaintingAreaContentPanelGrid.RenderTransform = rotateTransform;
            PocketPaintApplication.GetInstance().PaintingAreaContentPanelGrid.UpdateLayout();
            PocketPaintApplication.GetInstance().PaintingAreaContentPanelGrid.InvalidateArrange();
            PocketPaintApplication.GetInstance().PaintingAreaContentPanelGrid.InvalidateMeasure();
            //var rotated = new RotateTransform();
            //rotated.Angle = 90;
            //PocketPaintApplication.GetInstance().PaintingAreaContentPanelGrid.RenderTransform = rotated;
            //CommandManager.GetInstance().CommitCommand(new RotateCommand(RotateCommand.Direction.Right, _angle));
        }
    }
}