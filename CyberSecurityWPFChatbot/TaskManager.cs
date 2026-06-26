using System.Collections.Generic;
using System.Text;

namespace CyberSecurityWPFChatbot
{
    public class TaskManager
    {
        public List<string> Tasks =
            new List<string>();

        public void AddTask(string task)
        {
            Tasks.Add(task);
        }

        public string ViewTasks()
        {
            if (Tasks.Count == 0)
            {
                return "No tasks available.";
            }

            StringBuilder result =
                new StringBuilder();

            result.AppendLine("=== TASK LIST ===");

            for (int i = 0; i < Tasks.Count; i++)
            {
                result.AppendLine(
                    (i + 1) + ". " +
                    Tasks[i]);
            }

            return result.ToString();
        }

        public string DeleteTask(int index)
        {
            if (index >= 0 &&
                index < Tasks.Count)
            {
                string removedTask =
                    Tasks[index];

                Tasks.RemoveAt(index);

                return "Task removed: " +
                       removedTask;
            }

            return "Invalid task number.";
        }
    }
}