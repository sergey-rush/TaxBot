using System;
using System.IO;
using System.Text;

namespace TaxBot
{
    public static class Logger
    {
        private static readonly string ApplicationPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        private static string logFilePath;

        private static bool initialized;

        public static void Initialize()
        {
            if (MainForm.LogEnabled)
            {
                string logPath = Path.Combine(ApplicationPath, "Logs");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                ClearDirectory(logPath);

                string logFileName = "log_" + DateTime.Now.ToString("MMdd_HH.mm.ss") + ".txt";
                logFilePath = Path.Combine(logPath, logFileName);
                CreateAppLog(logFilePath);
                initialized = true;
            }
        }


        public static bool AppendAction(string appAction)
        {
            if (initialized)
            {
                Func<string, bool> a = Append;
                return a(appAction);
            }
            return false;
        }

        private static bool Append(string appAction)
        {
            bool result = false;
            try
            {
                using (var sw = new StreamWriter(logFilePath, true, Encoding.Unicode))
                {
                    sw.Write("\r\n" + DateTime.Now + "\t" + appAction);
                    result = true;
                }
            }
            catch { }

            return result;
        }

        private static void ClearDirectory(string logPath)
        {
            const int MaxFiles = 30;
            var directoryInfo = new DirectoryInfo(logPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            int fileInfosLength = fileInfos.Length;

            if (fileInfosLength > MaxFiles)
            {
                for (int i = MaxFiles; i < fileInfosLength; i++)
                {
                    fileInfos[i - MaxFiles].Delete();
                }
            }
        }

        private static void CreateAppLog(string p)
        {
            using (var sw = new StreamWriter(p, false, Encoding.Unicode))
            {
                sw.Write("\t\tTax Bot Service");
                sw.Write("\r\n");
                sw.Write("\r\nThis is an informational event log only. No user action is required.");
                sw.Write("\r\n\tCreated date: " + DateTime.Now);
                sw.Write("\r\n");
                sw.Write("\r\n\t-----------------------------------------------------------------------------");
                sw.Write("\r\n");
            }
        }
    }
}