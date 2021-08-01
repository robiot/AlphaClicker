using System.Windows;
using System.Windows.Input;

namespace AlphaClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChangeHotkey: Window
    {
        public ChangeHotkey()
        {
            InitializeComponent();
        }


        private void hotkeyWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private string CodeToSpecialKey(int key)
        {
            switch (key)
            {
                case 112:
                    return "F1";
                case 113:
                    return "F2";
                case 114:
                    return "F3";
                case 115:
                    return "F4";
                case 116:
                    return "F5";
                case 117:
                    return "F6";
                case 118:
                    return "F7";
                case 119:
                    return "F8";
                case 120:
                    return "F9";
                case 121:
                    return "F10";
                case 122:
                    return "F11";
                case 123:
                    return "F12";
            }
            return "";
        }

        private bool hasSpecKey = false;
        private int key1 = -1;
        private int key2 = -1;
        private void hotkeyWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!startBtn.IsEnabled)
            {
                for (int i = 8; i < 123; i++)
                {
                    if (WinApi.GetAsyncKeyState(i) > 0)
                    {
                        /* If Char Or Number */
                        if (i >= 48 && i <= 90)
                        {
                            keyBox.AppendText(((char)i).ToString());
                            startBtn.IsEnabled = true;
                            key2 = i;
                            break;
                        }
                        /* If function key */
                        else if (i >= 112 && i <= 123)
                        {
                            keyBox.AppendText(CodeToSpecialKey(i));
                            startBtn.IsEnabled = true;
                            key2 = i;
                            break;
                        }
                        /* If any extra keys (shift, ctrl, alt) */
                        else if (i >= 16 && i <= 18)
                        {
                            if (hasSpecKey == false)
                            {
                                keyBox.AppendText(e.Key.ToString().Replace("Left", "")
                                    .Replace("Right", "")+ " + ");
                                key1 = i;
                                hasSpecKey = true;
                            }
                        }
                    }


                }
            }
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            hasSpecKey = false;
            startBtn.IsEnabled = false;
            keyBox.Clear();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            Keybinds.key1 = key1;
            Keybinds.key2 = key2;
            Keybinds.keyBinding = keyBox.Text;
            AlphaRegistry.SetKeybindValues();

            ((MainWindow)this.Owner).LoadKeybind();
            Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void hotkeyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            keyBox.Text = Keybinds.keyBinding;
            ((MainWindow)this.Owner).keyEnabled = false;
        }

        private void hotkeyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)this.Owner).keyEnabled = true;
        }
    }

    public class Keybinds
    {
        // Default keybinds if reg fail
        public static int key1 = -1;
        public static int key2 = (int)VK.VK_F6;
        public static string keyBinding = "F6"; 
    }
}
