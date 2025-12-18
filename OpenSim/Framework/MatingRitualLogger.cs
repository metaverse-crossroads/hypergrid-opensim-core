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

        public static void Log(string actor, string action, string context = "")
        {
            string message = $"[MATING RITUAL] {actor.PadRight(15)} | {action}";
            if (!string.IsNullOrEmpty(context))
            {
                message += $" | {context}";
            }

            // Log to console/standard log for visibility
            m_log.Info(message);

            // Also append to a specific file for the report
            try
            {
                File.AppendAllText(LogPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}{Environment.NewLine}");
            }
            catch (Exception) { /* Best effort */ }
        }

        public static void Chapter(string title)
        {
            Log("NARRATOR", $"--- {title} ---");
        }
    }
}
