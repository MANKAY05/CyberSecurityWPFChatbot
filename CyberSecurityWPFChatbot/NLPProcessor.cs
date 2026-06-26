namespace CyberSecurityWPFChatbot
{
    public class NLPProcessor
    {
        public string ProcessInput(
            string input)
        {
            input =
                input.ToLower();

            if (input.Contains(
                "how do i protect my password"))
            {
                return "Use strong passwords, avoid sharing them, and enable two-factor authentication.";
            }

            if (input.Contains(
                "my account was hacked"))
            {
                return "Immediately change your password and enable 2FA.";
            }

            if (input.Contains(
                "is phishing dangerous"))
            {
                return "Yes. Phishing attacks can steal your personal information.";
            }

            if (input.Contains(
                "how can i stay safe online"))
            {
                return "Keep your software updated, use strong passwords, and avoid suspicious links.";
            }

            if (input.Contains(
                "tell me about malware"))
            {
                return "Malware is malicious software designed to damage or gain unauthorized access to systems.";
            }

            return "";
        }
    }
}