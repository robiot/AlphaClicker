using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlphaClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public void loadKeybind()
        {
            startBtn.Content = $"Start ({Keybinds.keyBinding})";
            stopBtn.Content = $"Stop ({Keybinds.keyBinding})";
        }

        private void cerror(string errormessage)
        {
            toggleClick();
            MessageBox.Show(errormessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private int toInt(string number)
        {
            return Int32.Parse((number == "") ? "0" : number);
        }


        private void fadeButtonColor(Button btn, string hex)
        {
            ColorAnimation animation =
               new ColorAnimation(
                   (Color)ColorConverter.ConvertFromString(hex),
                   new Duration(TimeSpan.FromSeconds(0.2))
               );
            btn.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        private void toggleClick()
        {
            string startEnabled = "#1494e3";
            string startDisabled = "#084466";

            string stopEnabled = "#FF605C";
            string stopDisabled = "#c43c35";


            if (startBtn.IsEnabled)
            {
                fadeButtonColor(startBtn, startDisabled);
                fadeButtonColor(stopBtn, stopEnabled);
                startBtn.IsEnabled = false;
                stopBtn.IsEnabled = true;

                Thread clickhandler = new Thread(clickHandler);
                clickhandler.Start();
            }
            else
            {
                fadeButtonColor(startBtn, startEnabled);
                fadeButtonColor(stopBtn, stopDisabled);

                startBtn.IsEnabled = true;
                stopBtn.IsEnabled = false;
            }
        }

        public bool keyEnabled = true;
        void keyHandler()
        {
            while (true)
            {
                if (keyEnabled)
                {
                    if (Keybinds.key1 == -1)
                    {
                        if (WinApi.GetAsyncKeyState(Keybinds.key2) > 0)
                        {
                            Dispatcher.Invoke((Action)(() =>
                            {
                                toggleClick();
                            }));
                        }
                    }
                    else
                    {
                        if ((WinApi.GetAsyncKeyState(Keybinds.key1) & 0x8000) == 0x8000 &&
                            WinApi.GetAsyncKeyState(Keybinds.key2) > 0)
                        {
                            Dispatcher.Invoke((Action)(() =>
                            {
                                toggleClick();
                            }));
                        }
                    }
                }
                Thread.Sleep(200);
            }

        }

        void clickHandler()
        {
            int sleep = 0;
            
            string mouseBtn = "";
            string clickType = "";

            bool repeatTimesChecked = false;
            int repeatTimes = 0;

            bool customCoordsChecked = false;
            int customCoordsX = 0, customCoordsY = 0; 

            Dispatcher.Invoke((Action)(() =>
            {
                /* Grab Click Interval */
                try
                {                    
                    sleep = toInt(millisecsBox.Text)
                    + toInt(secondsBox.Text) * 1000
                    + toInt(minsBox.Text) * 60000
                    + toInt(hoursBox.Text) * 3600000;
                    sleep = (sleep == 0) ? 1 : sleep;
                }

                catch (FormatException)
                {
                    cerror("Invalid Click Interval");
                    return;
                }

                /* Grab Mousebutton And Clicktype */
                mouseBtn = mouseBtnCBOX.Text;
                clickType = clickTypeCBOX.Text;

                /* Grab Repeat Stuff */
                repeatTimesChecked = (bool)repeatTimesRBtn.IsChecked;
                if (repeatTimesChecked)
                {
                    try
                    {
                        repeatTimes = Int32.Parse(repeatTimesBox.Text);
                    }
                    catch (FormatException)
                    {
                        cerror("Invalid Repeat Times Number");
                        return;
                    }
                }


                /* Grab Coords Stuff */
                customCoordsChecked = (bool)coordsCBtn.IsChecked;
                if (customCoordsChecked)
                {
                    try
                    {
                        customCoordsX = Int32.Parse(xBox.Text);
                        customCoordsY = Int32.Parse(yBox.Text);
                    }
                    catch (FormatException)
                    {
                        cerror("Invalid Repeat Times Number");
                        return;
                    }
                }
            }));

            int repeatCount = 0;
            while (true)
            {
                bool doClick = false;
                Dispatcher.Invoke((Action)(() =>
                {
                    doClick = stopBtn.IsEnabled;
                }));

                if (doClick)
                {
                    if (repeatTimesChecked)
                    {
                        if (repeatCount >= repeatTimes)
                        {
                            Dispatcher.Invoke((Action)(() =>
                            {
                                toggleClick();
                            }));
                            break;
                        }
                        repeatCount += 1;
                    }

                    if (clickType == "Single")
                    {
                        WinApi.DoClick(mouseBtn, customCoordsChecked, customCoordsX, customCoordsY);
                    }
                    else
                    {
                        WinApi.DoClick(mouseBtn, customCoordsChecked, customCoordsX, customCoordsY);
                        Thread.Sleep(400);
                        WinApi.DoClick(mouseBtn, customCoordsChecked, customCoordsX, customCoordsY);
                    }

                    Thread.Sleep(sleep);
                }
                else
                {
                    break;
                }
            }
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Thread keyhandler = new Thread(keyHandler);
            keyhandler.Start();

            loadKeybind();
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // So all threads close and not become background process
            Environment.Exit(0);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /* Top Bar */
        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void minimizeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleClick();
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleClick();
        }

        private void toggleTopmostBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Topmost)
            {
                toggleTopmostBtn.Content = "Toggle Topmost (Off)";
                this.Topmost = false;
            }
            else
            {
                toggleTopmostBtn.Content = "Toggle Topmost (On)";
                this.Topmost = true;
            }
        }

        private void changeHotkeyBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeHotkey win = new ChangeHotkey();
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Owner = this;
            win.ShowDialog();
        }
    }

    public class WinApi
    {
        /*
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }*/

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int character);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);

        // Get custom coords
        //[DllImport("user32.dll")]
        //public static extern bool GetCursorPos(out POINT lpPoint);

        /*public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }*/

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
