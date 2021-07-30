using System.IO;
using CoolParking.BL.Models;
using CoolParking.BL.Interfaces;

namespace CoolParking.BL.Services
{
    public class LogService : ILogService
    {
        public string LogPath { get; private set; }

        public LogService() : this(Settings.InitialLogWritingPath) { }

        public LogService(string logPath)
        {
            this.LogPath = logPath;
        }

        /// <summary>
        /// Read transactions from log file
        /// </summary>
        /// <returns>Data from log file</returns>
        public string Read()
        {
            string temp = "";
            using (var file = new StreamReader(LogPath))
            {
                temp = file.ReadToEnd();
            }
            return temp;
        }

        /// <summary>
        /// Write transactions to log file
        /// </summary>
        /// <param name="logInfo">Data for writing in the file</param>
        public void Write(string logInfo)
        {
            if (logInfo.Length == 0) return;
            using (var file = new StreamWriter(LogPath, true))
            {
                file.WriteLine(logInfo);
            }
        }
    }
}