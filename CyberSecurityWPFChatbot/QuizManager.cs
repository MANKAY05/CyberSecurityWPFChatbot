using System.Collections.Generic;
using System.Text;

namespace CyberSecurityWPFChatbot
{
    public class QuizManager
    {
        public int CurrentQuestion = 0;

        public int Score = 0;

        public List<string> UserAnswers =
            new List<string>();

        public List<string> Questions =
            new List<string>()
        {
            "Q1: What does 2FA stand for?",

            "Q2: True or False - You should share your password with trusted friends.",

            "Q3: Which of the following is a phishing attack?\nA. Fake email\nB. Software update\nC. Antivirus scan",

            "Q4: What is malware?",

            "Q5: True or False - Public WiFi is always safe.",

            "Q6: Which one is a strong password?\nA. 123456\nB. password\nC. P@ssw0rd#2025",

            "Q7: What does VPN stand for?",

            "Q8: True or False - Antivirus software helps protect a computer.",

            "Q9: What is the purpose of a firewall?",

            "Q10: Which attack tries to trick users into revealing information?\nA. Phishing\nB. Firewall\nC. Encryption",

            "Q11: True or False - Software updates improve security.",

            "Q12: Why is cybersecurity important?"
        };

        public List<string> Answers =
            new List<string>()
        {
            "two factor authentication",
            "false",
            "a",
            "malicious software",
            "false",
            "c",
            "virtual private network",
            "true",
            "protect network",
            "a",
            "true",
            "protect information"
        };

        public void CheckAnswer(string answer)
        {
            UserAnswers.Add(answer);

            answer =
                answer.ToLower().Trim();

            string correct =
                Answers[CurrentQuestion];

            if (answer.Contains(correct))
            {
                Score++;
            }
        }

        public string GetCorrections()
        {
            StringBuilder report =
                new StringBuilder();

            report.AppendLine(
                "===== QUIZ RESULTS =====");

            report.AppendLine(
                "Score: " +
                Score +
                "/" +
                Questions.Count);

            double percentage =
                ((double)Score /
                 Questions.Count) * 100;

            report.AppendLine(
                "Percentage: " +
                percentage.ToString("F0") +
                "%");

            report.AppendLine("");

            report.AppendLine(
                "===== CORRECTIONS =====");

            for (int i = 0;
                 i < Questions.Count;
                 i++)
            {
                report.AppendLine(
                    Questions[i]);

                report.AppendLine(
                    "Your Answer: " +
                    UserAnswers[i]);

                report.AppendLine(
                    "Correct Answer: " +
                    Answers[i]);

                report.AppendLine("");
            }

            return report.ToString();
        }

        public void ResetQuiz()
        {
            CurrentQuestion = 0;

            Score = 0;

            UserAnswers.Clear();
        }
    }
}