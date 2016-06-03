using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace UnitTestUtilities
{
    public static class GeneralUtilities
    {
        public static bool WaitUntil(Func<bool> func)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(100);
                if (func())
                {
                    return true;
                }
                //TODO: add timeout
            }

            return false;
        }

        public static Rect GetScreenPos(UIElement element)
        {
            PresentationSource source = PresentationSource.FromVisual(element);
            if (source == null)
            {
                return new Rect();
            }

            Matrix transformToDevice = source.CompositionTarget.TransformToDevice;

            Size size = (Size)transformToDevice.Transform((Vector)element.RenderSize);

            Point pointToScreen = element.PointToScreen(new Point(0, 0));
            return new Rect(pointToScreen.X, pointToScreen.Y, size.Width, size.Height);
        }

        public static Point GetMiddleInScreenCoordinates(UIElement element)
        {
            Rect position = GetScreenPos(element);
            return new Point(position.X + (position.Width / 2), position.Y + (position.Height / 2));
        }
    }
}
