using System;
using System.Threading;
using System.Windows;

namespace AlphaClicker
{
    public partial class GetCursorPos: Window
    {
        public GetCursorPos()
        {
            InitializeComponent();
        }

        private bool enabled = true;
        private void WindowHandler()
        {            
            while (enabled)
            {
                Point location = WinApi.GetCursorPosition();

                Dispatcher.Invoke((Action)(() =>
                {
                    this.Left = location.X;
                    this.Top = location.Y;
                    xLbl.Content = $"X: {location.X}";
                    yLbl.Content = $"Y: {location.Y}";

                }));
                Thread.Sleep(20);
            }
        }

        private void ClickHandler()
        {
            while (enabled)
            {
                if (WinApi.GetAsyncKeyState((int)VK.VK_LBUTTON) > 0||
                        WinApi.GetAsyncKeyState((int)VK.VK_ESCAPE) > 0)
                {
                    enabled = false;
                    Dispatcher.Invoke((Action)(() =>
                    {
                        Point location = WinApi.GetCursorPosition();

                        ((MainWindow)this.Owner).xBox.Text = location.X.ToString();
                        ((MainWindow)this.Owner).yBox.Text = location.Y.ToString();
                        ((MainWindow)this.Owner).WindowState = WindowState.Normal;
                        ((MainWindow)this.Owner).keyEnabled = true;
                        Close();
                    }));
                }
                Thread.Sleep(200);
            }
        }

        private void getCursorPosWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((MainWindow)this.Owner).keyEnabled = false;

            Thread windowHandler = new Thread(WindowHandler);
            windowHandler.Start();

            Thread clickhandler = new Thread(ClickHandler);
            clickhandler.Start();
        }
    }
}
