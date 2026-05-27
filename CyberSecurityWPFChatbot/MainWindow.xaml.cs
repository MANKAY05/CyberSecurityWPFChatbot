using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Speech.Synthesis;

namespace CyberSecurityWPFChatbot
{
    public partial class MainWindow : Window
    {
        // MEMORY FEATURE 
        Dictionary<string, string> memory =
            new Dictionary<string, string>();

        // GENERIC COLLECTION 
        List<string> greetings =
            new List<string>()
        {
            "hello",
            "hi",
            "hey"
        };

        // CYBERSECURITY RESPONSES
        Dictionary<string, string> cyberResponses =
            new Dictionary<string, string>()
        {
            {"password", "Always use strong passwords."},

            {"phishing", "Avoid suspicious emails and links."},

            {"virus", "Install antivirus software."},

            {"2fa", "Two factor authentication improves security."},

            {"hacker", "Hackers attempt unauthorized access."}
        };

        // DELEGATE
        delegate string SentimentDelegate(string message);

        // CONSTRUCTOR
        public MainWindow()  
        {
            InitializeComponent();
             
            // PLAY GREETING VOICE
            PlayGreeting();

            // WELCOME MESSAGE
            AddMessage( 
                "Bot: Hello! I am your Cybersecurity Assistant.");
        }

        // VOICE GREETING METHOD
        private void PlayGreeting()
        {
            SpeechSynthesizer synthesizer =
                new SpeechSynthesizer();
            synthesizer.SpeakAsync("Hello Welcome to Moilwa's Cybersecurity Chatbot!"); 
        }

        // SEND BUTTON EVENT
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string userMessage =
                txtMessage.Text.ToLower();

            // EMPTY MESSAGE CHECK
            if (userMessage == "")
            {
                MessageBox.Show(
                    "Please enter a message.");

                return;
            }

            // DISPLAY USER MESSAGE
            AddMessage(
                "You: " + userMessage);

            // SENTIMENT DETECTION
            SentimentDelegate sentiment =
                DetectSentiment;

            string moodResponse =
                sentiment(userMessage);

            if (moodResponse != "")
            {
                AddMessage(
                    "Bot: " + moodResponse);
            }

            // MEMORY FEATURE
            if (userMessage.Contains("my name is"))
            {
                string name =
                    userMessage.Replace(
                        "my name is", "").Trim();

                memory["username"] = name;

                AddMessage(
                    "Bot: Nice to meet you "
                    + name);
            }

            // RECALL MEMORY
            else if (userMessage.Contains(
                "what is my name"))
            {
                if (memory.ContainsKey("username"))
                {
                    AddMessage(
                        "Bot: Your name is "
                        + memory["username"]);
                }
                else
                {
                    AddMessage(
                        "Bot: I do not know your name yet.");
                }
            }

            // GREETING DETECTION
            else if (greetings.Any(
                g => userMessage.Contains(g)))
            {
                AddMessage(
                    "Bot: Hello! How can I help you?");
            }

            // CYBERSECURITY RESPONSES
            else
            {
                bool found = false;

                foreach (var item in cyberResponses)
                {
                    if (userMessage.Contains(item.Key))
                    {
                        AddMessage(
                            "Bot: " + item.Value);

                        found = true;
                        break;
                    }
                }

                // DEFAULT RESPONSE
                if (!found)
                {
                    AddMessage(
                        "Bot: Ask me something about cybersecurity.");
                }
            }

            // CLEAR INPUT BOX
            txtMessage.Clear();
        }

        // SENTIMENT DETECTION METHOD
        private string DetectSentiment(string message)
        {
            if (message.Contains("sad") ||
                message.Contains("angry"))
            {
                return "I am sorry you feel that way.";
            }

            else if (message.Contains("happy") ||
                     message.Contains("good"))
            {
                return "That is great to hear!";
            }

            return "";
        }

        // ADD TEXT TO CHAT AREA
        private void AddMessage(string message)
        {
            Paragraph paragraph =
                new Paragraph();

            paragraph.Inlines.Add(message);

            rtbChat.Document.Blocks.Add(paragraph);

            // AUTO SCROLL TO LATEST MESSAGE
            rtbChat.ScrollToEnd();
        }
    }
}