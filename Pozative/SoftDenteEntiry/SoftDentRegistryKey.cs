using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public static class SoftDentRegistryKey
    {
        public static string ServerExeDir
        {
            get
            {
                string str = SoftDentRegistryKey.GetValue(Registry.LocalMachine, "SOFTWARE\\WOW6432Node\\PWInc\\SoftDent", "ServerExeDir");
                if (str == "")
                    str = SoftDentRegistryKey.GetValue(Registry.LocalMachine, "SOFTWARE\\PWInc\\Demo\\SoftDent", "ServerExeDir");
                if(str == "")
                    str = SoftDentRegistryKey.GetValue(Registry.LocalMachine, "SOFTWARE\\PWInc\\SoftDent", "ServerExeDir");
                return str;
            }
        }

        public static string LocalExeDir
        {
            get
            {
                string str = SoftDentRegistryKey.GetValue(Registry.LocalMachine, "SOFTWARE\\PWInc\\SoftDent", "LocalExeDir");
                if (str == "")
                    str = SoftDentRegistryKey.GetValue(Registry.LocalMachine, "SOFTWARE\\PWInc\\Demo\\SoftDent", "LocalExeDir");
                return str;
            }
        }

        public static string ServerExeUNCDir
        {
            get
            {
                return SoftDentRegistryKey.GetValue(Registry.LocalMachine, "SOFTWARE\\PWInc\\SoftDent", "ServerExeUNCDir");
            }
            set
            {
                SoftDentRegistryKey.SetValue(Registry.LocalMachine, "SOFTWARE\\PWInc\\SoftDent", "ServerExeUNCDir", value);
            }
        }

        public static string FaircomServerName
        {
            get
            {
                object valueObject = SoftDentRegistryKey.GetValueObject(Registry.CurrentUser, "Software\\InfoSoft\\SoftDent\\Client", "ServerName");
                if (valueObject != null)
                    return Encoding.ASCII.GetString((byte[])valueObject).Replace("\0", "");
                return "";
            }
        }

        public static string FaircomServerNameHKLM
        {
            get
            {
                return SoftDentRegistryKey.GetValue(Registry.LocalMachine, "SOFTWARE\\PWInc\\SoftDent", "FaircomServerName");
            }
            set
            {
                SoftDentRegistryKey.SetValue(Registry.LocalMachine, "SOFTWARE\\PWInc\\SoftDent", "FaircomServerName", value);
            }
        }

        public static bool InteropLogging
        {
            get
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\PWInc\\SoftDent");
                if (registryKey == null)
                    return false;
                object obj = registryKey.GetValue("InteropLog", (object)"0");
                registryKey.Close();
                return Convert.ToInt32(obj) != 0;
            }
        }

        private static string GetValue(RegistryKey key, string keyName, string keyValue)
        {
            object valueObject = SoftDentRegistryKey.GetValueObject(key, keyName, keyValue);
            if (valueObject != null)
                return valueObject.ToString();
            return "";
        }

        private static object GetValueObject(RegistryKey key, string keyName, string keyValue)
        {
            return key.OpenSubKey(keyName, false).GetValue(keyValue);
        }

        private static void SetValue(RegistryKey key, string keyName, string keyValue, string value)
        {
            SoftDentRegistryKey.SetValue(key, keyName, keyValue, (object)value, RegistryValueKind.String);
        }

        private static void SetValue(
          RegistryKey key,
          string keyName,
          string keyValue,
          object value,
          RegistryValueKind kind)
        {
            key.OpenSubKey(keyName, true).SetValue(keyValue, value, kind);
        }
    }
}
