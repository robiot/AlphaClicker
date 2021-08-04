using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace AlphaClicker
{
    public class WinApi
    {
        
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int character);

        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);

        // Get custom coords
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        public static void DoClick(string button, bool useCustomCoords, int X, int Y)
        {
            if (useCustomCoords) { SetCursorPos(X, Y); }
            switch (button)
            {
                case "Left":
                    mouse_event((uint)MOUSEEVENTF.LEFTDOWN | (uint)MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
                    break;
                case "Right":
                    mouse_event((uint)MOUSEEVENTF.RIGHTDOWN | (uint)MOUSEEVENTF.RIGHTUP, 0, 0, 0, 0);
                    break;
                case "Middle":
                    mouse_event((uint)MOUSEEVENTF.MIDDLEDOWN | (uint)MOUSEEVENTF.MIDDLEUP, 0, 0, 0, 0);
                    break;
            }
        }
    }
}
