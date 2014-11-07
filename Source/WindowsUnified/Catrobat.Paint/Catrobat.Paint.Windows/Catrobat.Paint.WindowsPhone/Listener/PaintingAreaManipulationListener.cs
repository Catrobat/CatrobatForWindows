﻿using System;
using System.Windows;
using System.Windows.Input;
using Catrobat.Paint.Phone.Tool;
using Catrobat.Paint.Phone.Ui;
using Windows.UI.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Catrobat.Paint.WindowsPhone.Tool;
using Windows.UI.Xaml.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;

namespace Catrobat.Paint.Phone.Listener
{
    class PaintingAreaManipulationListener 
    {
        Point lastPoint = new Point(0.0, 0.0);
        int lastAngleValue = 0;
        RotateTransform rotate = new RotateTransform();

        public void ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            e.Mode.ToString();
        }
        
        public void ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            var point = new Point(Convert.ToInt32(e.Position.X), Convert.ToInt32(e.Position.Y));

            // TODO some bubbling? issue here, fast multiple applicationbartop undos result in triggering this event
            if (point.X < 0 || point.Y < 0 || Spinner.SpinnerActive || e.Handled)
            {
                return;
            }

            PocketPaintApplication.GetInstance().ToolCurrent.HandleDown(point);
        }

        public void ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var point = new Point(Convert.ToInt32(e.Position.X), Convert.ToInt32(e.Position.Y));

            // TODO some bubbling? issue here, fast multiple applicationbartop undos result in triggering this event
            if (point.X < 0 || point.Y < 0 || Spinner.SpinnerActive || e.Handled)
            {
                return;
            }


            object movezoom = null;

            RotateTransform rotate = new RotateTransform();

            if (PocketPaintApplication.GetInstance().ToolCurrent.GetToolType() == ToolType.Rect)
            {
                //rotate.CenterX = PocketPaintApplication.GetInstance().GridRectangleSelectionControl.Width / 2.0;
                //rotate.CenterY = PocketPaintApplication.GetInstance().GridRectangleSelectionControl.Height / 2.0;
                Point rotateCenterPoint = PocketPaintApplication.GetInstance().RectangleSelectionControl.getCenterOfGridMain();
                rotate.CenterX = rotateCenterPoint.X;
                rotate.CenterY = rotateCenterPoint.Y;

                Point centerPoint = PocketPaintApplication.GetInstance().RectangleSelectionControl.getCenterCoordinateOfGridMain();  
                
                if (!(lastPoint.X == 0.0 && lastPoint.Y == 0.0) &&
                    (lastPoint.X != point.X || lastPoint.Y != point.Y))
                {
                    double currentXLength = point.X - centerPoint.X;
                    double currentYLength = point.Y - centerPoint.Y;

                    double previousXLength = lastPoint.X - centerPoint.X;
                    double previousYLength = lastPoint.Y - centerPoint.Y;

                    double deltaAngle = (Math.Atan(previousXLength / previousYLength) - Math.Atan(currentXLength / currentYLength));
                    double rotationAngle = deltaAngle * 180.0 / Math.PI;

                    rotate.Angle = rotationAngle;
                }
                lastPoint = point;
            }
            else
            {
                if (e.Delta.Scale != 1.0)
                {

                    movezoom = new ScaleTransform();
                    if (e.Delta.Scale > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Scale " + e.Delta.Scale);
                        ((ScaleTransform)movezoom).ScaleX *= e.Delta.Scale;
                        ((ScaleTransform)movezoom).ScaleY *= e.Delta.Scale;
                    }
                }
                else
                {
                    movezoom = new TranslateTransform();

                    ((TranslateTransform)movezoom).X += e.Delta.Translation.X;
                    ((TranslateTransform)movezoom).Y += e.Delta.Translation.Y;
                }
            }
            

            switch (PocketPaintApplication.GetInstance().ToolCurrent.GetToolType())
            {
                case ToolType.Brush:
                case ToolType.Eraser:
                    PocketPaintApplication.GetInstance().ToolCurrent.HandleMove(point);
                    break;
                case ToolType.Cursor:
                    PocketPaintApplication.GetInstance().ToolCurrent.HandleMove(movezoom);
                    PocketPaintApplication.GetInstance().ToolCurrent.HandleMove(point);
                    break;
                case ToolType.Move:
                case ToolType.Zoom:
                    PocketPaintApplication.GetInstance().ToolCurrent.HandleMove(movezoom);
                    break;
                case ToolType.Line:
                    PocketPaintApplication.GetInstance().ToolCurrent.HandleMove(point);
                    break;
                case ToolType.Rect:
                    if (rotate.Angle != 0)
                    {
                    PocketPaintApplication.GetInstance().ToolCurrent.HandleMove(rotate);
                    }
                    break;
            }
        }


        public void ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var point = new Point(Convert.ToInt32(e.Position.X), Convert.ToInt32(e.Position.Y));

            // TODO some bubbling? issue here, fast multiple applicationbartop undos result in triggering this event
            if (point.X < 0 || point.Y < 0 || Spinner.SpinnerActive || e.Handled)
            {
                return;
            }

            PocketPaintApplication.GetInstance().ToolCurrent.HandleUp(point);        
        }

        public void ResetDrawingSpace()
        {
            PocketPaintApplication.GetInstance().ToolCurrent.ResetDrawingSpace();
        }
    }
}
