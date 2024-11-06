using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public static class SoftDentVersion
    {
        private static string _SoftDentPath;
        private static int _MajorPart;
        private static int _MinorPart;
        private static int _BuildPart;
        private static int _PrivatePart;
        private static bool _VersionInfoRetrieved;
        private static bool _IsClientServer;
        private static bool _IsClientServerRetrieved;

        public static int MajorPart
        {
            get
            {
                if (!SoftDentVersion._VersionInfoRetrieved)
                    SoftDentVersion.GetVersionInfo();
                return SoftDentVersion._MajorPart;
            }
        }

        public static int MinorPart
        {
            get
            {
                if (!SoftDentVersion._VersionInfoRetrieved)
                    SoftDentVersion.GetVersionInfo();
                return SoftDentVersion._MinorPart;
            }
        }

        public static int BuildPart
        {
            get
            {
                if (!SoftDentVersion._VersionInfoRetrieved)
                    SoftDentVersion.GetVersionInfo();
                return SoftDentVersion._BuildPart;
            }
        }

        public static int PrivatePart
        {
            get
            {
                if (!SoftDentVersion._VersionInfoRetrieved)
                    SoftDentVersion.GetVersionInfo();
                return SoftDentVersion._PrivatePart;
            }
        }

        public static bool IsClientServer
        {
            get
            {
                if (!SoftDentVersion._IsClientServerRetrieved)
                {
                    SoftDentVersion._IsClientServer = SoftDentVersion.GetIsClientServer();
                    SoftDentVersion._IsClientServerRetrieved = true;
                }
                return SoftDentVersion._IsClientServer;
            }
        }

        public static void Init(string path)
        {
            if (!string.IsNullOrEmpty(SoftDentVersion._SoftDentPath))
                throw new Exception("SoftDentVersion: Already initialized. Unexpected behavior may result");
            SoftDentVersion._SoftDentPath = path;
        }

        private static bool GetIsClientServer()
        {
            if (string.IsNullOrEmpty(SoftDentVersion._SoftDentPath))
                throw new ArgumentNullException("SoftDentVersion - Must call Init()");
            bool flag = false;
            if (File.Exists(SoftDentVersion._SoftDentPath + "\\SDWinCS.exe"))
                flag = true;
            return flag;
        }

        private static void GetVersionInfo()
        {
            string softDentPath = SoftDentVersion._SoftDentPath;
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(!SoftDentVersion.IsClientServer ? softDentPath + "\\SDWin.exe" : softDentPath + "\\SDWinCS.exe");
            SoftDentVersion._MajorPart = versionInfo.FileMajorPart;
            SoftDentVersion._MinorPart = versionInfo.FileMinorPart;
            SoftDentVersion._BuildPart = versionInfo.FileBuildPart;
            SoftDentVersion._PrivatePart = versionInfo.FilePrivatePart;
            SoftDentVersion._VersionInfoRetrieved = true;
        }
    }
}
