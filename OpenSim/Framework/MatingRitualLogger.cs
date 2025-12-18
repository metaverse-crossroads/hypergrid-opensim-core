using System;
using System.IO;
using System.Text;
using log4net;
using System.Reflection;

namespace OpenSim.Framework
{
    public static class MatingRitualLogger
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string LogPath = "mating_rituals.log";

        public static void Log(string side, string component, string signal, string payload = "")
        {
            string message = $"[MATING RITUAL] [{side}] [{component}] {signal}";
            if (!string.IsNullOrEmpty(payload))
            {
                message += $" | {payload}";
            }

            // Log to console/standard log for visibility
            m_log.Info(message);

            // Also append to a specific file for the report
            try
            {
                File.AppendAllText(LogPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}{Environment.NewLine}");
            }
            catch (Exception) { /* Best effort */ }
        }
    }
}
