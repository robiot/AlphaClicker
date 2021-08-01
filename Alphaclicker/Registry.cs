using Microsoft.Win32;
using System;

namespace AlphaClicker
{
    public class AlphaRegistry
    {
        private static RegistryKey GetRegistryKey(string keyPath)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, true);
           
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey(keyPath);
            }

            return key;
        }

        private static int KeyToInt(string name)
        {
            RegistryKey regKey = GetRegistryKey("Software\\AlphaClicker");
            try
            {
                return Int32.Parse(regKey.GetValue(name).ToString());
            }
            catch
            {
                return -1;
            }
        }

        public static void SetKeybindValues()
        {
            RegistryKey regKey = GetRegistryKey("Software\\AlphaClicker");
            regKey.SetValue("key1", Keybinds.key1);
            regKey.SetValue("key2", Keybinds.key2);
            regKey.SetValue("keybinding", Keybinds.keyBinding);
            //messagebox.Show(KeyToInt("key1"));
        }

        public static void GetKeybindValues()
        {
            try
            {
                Keybinds.key1 = KeyToInt("key1");
                Keybinds.key2 = KeyToInt("key2");
                Keybinds.keyBinding = GetRegistryKey("Software\\AlphaClicker")
                    .GetValue("keybinding").ToString();
            }
            catch
            {

            }
        }
    }

}
