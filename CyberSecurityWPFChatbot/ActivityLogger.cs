using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityWPFChatbot
{
    public class ActivityLogger
    {
        private List<string> logs =
            new List<string>();

        public void AddLog(string activity)
        {
            logs.Add(
                DateTime.Now.ToString(
                "yyyy-MM-dd HH:mm:ss")
                + " - " +
                activity);
        }

        public string ViewLogs()
        {
            if (logs.Count == 0)
            {
                return "No activities recorded.";
            }

            StringBuilder result =
                new StringBuilder();

            result.AppendLine(
                "=== ACTIVITY LOG ===");

            foreach (string log in logs)
            {
                result.AppendLine(log);
            }

            return result.ToString();
        }
    }
}