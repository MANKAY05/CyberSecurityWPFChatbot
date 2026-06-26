using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CyberSecurityWPFChatbot
{
    public partial class MainWindow : Window
    {
        // MEMORY FEATURE
        Dictionary<string, string> memory =
            new Dictionary<string, string>();

        // GREETINGS
        List<string> greetings =
            new List<string>()
        {
            "hello",
            "hi",
            "hey"
        };

        // CYBERSECURITY RESPONSES
        Dictionary<string, List<string>> cyberResponses =
            new Dictionary<string, List<string>>()
        {
            {
                "password",
                new List<string>()
                {
                    "Always use strong passwords with symbols and numbers.",
                    "Avoid using your name or birth date as a password.",
                    "Change your passwords regularly for better security."
                }
            },

            {
                "phishing",
                new List<string>()
                {
                    "Be careful of fake emails asking for personal information.",
                    "Never click suspicious links from unknown senders.",
                    "Scammers often pretend to be banks or trusted organisations.",
                    "Always check the sender email address carefully.",
                    "Do not share passwords through email messages."
                }
            },

            {
                "virus",
                new List<string>()
                {
                    "Install antivirus software and keep it updated.",
                    "Avoid downloading files from unknown websites.",
                    "Viruses can damage your files and steal information."
                }
            },

            {
                "2fa",
                new List<string>()
                {
                    "Two-factor authentication improves account security.",
                    "2FA adds an extra verification step when logging in.",
                    "Enable 2FA on important accounts like email and banking."
                }
            },

            {
                "privacy",
                new List<string>()
                {
                    "Review your social media privacy settings regularly.",
                    "Avoid sharing sensitive information online.",
                    "Privacy helps protect your personal information."
                }
            },

            {
                "hacker",
                new List<string>()
                {
                    "Hackers try to gain unauthorized access to systems.",
                    "Avoid weak passwords to reduce hacking risks.",
                    "Keep your software updated to prevent attacks."
                }
            }
        };

        // RANDOM OBJECT
        Random random = new Random();

        // CURRENT TOPIC
        string currentTopic = "";

        // USER NAME CHECK
        bool userNameSaved = false;

        // DELEGATE
        delegate string SentimentDelegate(string message);

        // VOICE SYNTHESIS
        SpeechSynthesizer synthesizer =
            new SpeechSynthesizer();

        DatabaseHelper database =
    new DatabaseHelper();
        TaskManager taskManager =
    new TaskManager();

        QuizManager quizManager =
            new QuizManager();

        ActivityLogger logger =
            new ActivityLogger();

        NLPProcessor nlp =
            new NLPProcessor();

        bool quizStarted = false;
       

        // CONSTRUCTOR
        public MainWindow()
        {
            InitializeComponent();

            // VOICE SETTINGS
            synthesizer.Volume = 100;
            synthesizer.Rate = 0;

            // BOT GREETING
            AddBotMessage(
                "Hello! Welcome to MOILWA'S CYBER BOT.");

            AddBotMessage(
                "Before we begin, what is your name?");
        }
        private void btnHistory_Click(
    object sender,
    RoutedEventArgs e)
        {
            HistoryWindow history =
                new HistoryWindow();

            history.ShowDialog();
        }

        // SEND BUTTON EVENT
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string userMessage =
                txtMessage.Text.Trim().ToLower();

            // EMPTY CHECK
            if (userMessage == "")
            {
                MessageBox.Show(
                    "Please enter a message.");

                return;
            }

            // DISPLAY USER MESSAGE
            AddUserMessage(userMessage);
            logger.AddLog(
    "User entered: " +
    userMessage);

            database.SaveActivity(
                "User entered: " +
                userMessage);

            string nlpResponse =
                nlp.ProcessInput(userMessage);

            if (nlpResponse != "")
            {
                AddBotMessage(nlpResponse);

                logger.AddLog(
                    "NLP Response Generated");

                database.SaveActivity(
                    "NLP Response Generated");

                txtMessage.Clear();

                return;
            }



            // SAVE USER NAME
            if (!userNameSaved)
            {
                memory["username"] = userMessage;

                userNameSaved = true;

                AddBotMessage(
                    "Nice to meet you!");

                AddBotMessage(
                    "What cybersecurity topic would you like to learn about?");

                txtMessage.Clear();

                return;
            }

            // SENTIMENT DETECTION
            SentimentDelegate sentiment =
                DetectSentiment;

            string moodResponse =
                sentiment(userMessage);

            if (moodResponse != "")
            {
                AddBotMessage(moodResponse);
            }
            if (userMessage.StartsWith("add task"))
            {
                string task =
                    userMessage
                    .Replace("add task", "")
                    .Trim();

                taskManager.AddTask(task);

                AddBotMessage(
                    "Task added successfully.");

                logger.AddLog(
                    "Task Added");

                database.SaveActivity(
                    "Task Added");

                txtMessage.Clear();

                return;
            }

            if (quizStarted)
            {
                quizManager.CheckAnswer(
                    userMessage);

                quizManager.CurrentQuestion++;

                if (quizManager.CurrentQuestion <
                    quizManager.Questions.Count)
                {
                    AddBotMessage(
                        quizManager.Questions[
                            quizManager.CurrentQuestion]);
                }
                else
                {
                    AddBotMessage(
                        quizManager.GetCorrections());

                    logger.AddLog(
                        "Quiz Completed");

                    database.SaveActivity(
                        "Quiz Completed");

                    quizStarted = false;

                    quizManager.ResetQuiz();
                }

                txtMessage.Clear();

                return;
            }
            if (userMessage == "start quiz")
            {
                quizStarted = true;

                quizManager.CurrentQuestion = 0;

                quizManager.Score = 0;

                AddBotMessage(
                    "Cybersecurity Quiz Started!");

                AddBotMessage(
                    quizManager.Questions[0]);

                logger.AddLog(
                    "Quiz Started");

                database.SaveActivity(
                    "Quiz Started");

                txtMessage.Clear();

                return;
            }


            // USER INTEREST MEMORY
            else if (userMessage.Contains("i'm interested in") ||
                     userMessage.Contains("i am interested in"))
            {
                string topic =
                    userMessage
                    .Replace("i'm interested in", "")
                    .Replace("i am interested in", "")
                    .Trim();

                memory["interest"] = topic;

                AddBotMessage(
                    "Great! I will remember that you are interested in " +
                    topic + ".");

                AddBotMessage(
                    "Learning about " +
                    topic +
                    " can help you stay safer online.");
            }

            // RECALL USER INTEREST
            else if (userMessage.Contains("what am i interested in"))
            {
                if (memory.ContainsKey("interest"))
                {
                    AddBotMessage(
                        "You are interested in " +
                        memory["interest"]);
                }
                else
                {
                    AddBotMessage(
                        "You have not shared your interests yet.");
                }
            }

            // GREETINGS
            else if (greetings.Any(
                g => userMessage.Contains(g)))
            {
                AddBotMessage(
                    "Hello! How can I help you today?");
            }
            if (userMessage == "show logs")
            {
                AddBotMessage(
                    logger.ViewLogs());

                txtMessage.Clear();

                return;
            }

            // FOLLOW-UP CONVERSATION
            else if (userMessage.Contains("tell me more") ||
                     userMessage.Contains("another tip") ||
                     userMessage.Contains("explain more") ||
                     userMessage.Contains("continue"))
            {
                if (currentTopic != "")
                {
                    GiveCyberResponse(currentTopic);
                }
                else
                {
                    AddBotMessage(
                        "Please choose a cybersecurity topic first.");
                }
            }

            // PHISHING SPECIAL HANDLING
            else if (userMessage.Contains("phishing scam") ||
                     userMessage.Contains("email scam") ||
                     userMessage.Contains("fake email"))
            {
                currentTopic = "phishing";

                AddBotMessage(
                    "Phishing scams are dangerous because attackers try to trick users into revealing personal information.");

                string botReply =
     GiveCyberResponse("phishing");

                database.SaveChat(
                    userMessage,
                    botReply);
            }

            // CYBERSECURITY RESPONSES
            else
            {
                bool found = false;

                foreach (var item in cyberResponses)
                {
                    if (userMessage.Contains(item.Key))
                    {
                        currentTopic = item.Key;

                        string botReply =
     GiveCyberResponse(item.Key);

                        database.SaveChat(
                            userMessage,
                            botReply);

                        found = true;

                        break;
                    }
                }

                // DEFAULT RESPONSE
                if (!found)
                {
                    AddBotMessage(
                        "I am not sure I understand.");

                    AddBotMessage(
                        "Try asking about passwords, phishing, viruses, privacy, hackers or 2FA.");
                }
            }

            // CLEAR INPUT
            txtMessage.Clear();
        }

        // RANDOM RESPONSE METHOD
        private string GiveCyberResponse(
     string topic)
        {
            List<string> responses =
                cyberResponses[topic];

            int index =
                random.Next(
                    responses.Count);

            string selectedResponse =
                responses[index];

            AddBotMessage(
                selectedResponse);

            logger.AddLog(
                "Cyber Response: " +
                topic);

            database.SaveActivity(
                "Cyber Response: " +
                topic);

            return selectedResponse;
        }

        // SENTIMENT DETECTION
        private string DetectSentiment(string message)
        {
            // WORRIED
            if (message.Contains("worried") ||
                message.Contains("scared") ||
                message.Contains("frustrated"))
            {
                return
                    "It is understandable to feel that way. Cyber threats can be stressful, but there are ways to stay protected.";
            }

            // SAD OR ANGRY
            else if (message.Contains("sad") ||
                     message.Contains("angry"))
            {
                return
                    "I am sorry you feel that way. Let me help you with some cybersecurity advice.";
            }

            // HAPPY
            else if (message.Contains("happy") ||
                     message.Contains("good"))
            {
                return
                    "That is great to hear!";
            }

            // CURIOUS
            else if (message.Contains("curious"))
            {
                return
                    "Curiosity is important when learning cybersecurity.";
            }

            return "";
        }

        // BOT MESSAGE - LEFT SIDE
        private void AddBotMessage(string message)
        {
            Paragraph paragraph =
                new Paragraph();

            paragraph.TextAlignment =
                TextAlignment.Left;

            paragraph.Margin =
                new Thickness(10);

            Border border =
                new Border();

            border.Background =
                Brushes.DarkSlateBlue;

            border.CornerRadius =
                new CornerRadius(10);

            border.Padding =
                new Thickness(10);

            TextBlock text =
                new TextBlock();

            text.Text =
                "Bot: " + message;

            text.Foreground =
                Brushes.White;

            text.TextWrapping =
                TextWrapping.Wrap;

            border.Child = text;

            InlineUIContainer container =
                new InlineUIContainer(border);

            paragraph.Inlines.Add(container);

            rtbChat.Document.Blocks.Add(paragraph);

            rtbChat.ScrollToEnd();

            // BOT VOICE
            synthesizer.SpeakAsync(message);
        }

        // USER MESSAGE - RIGHT SIDE
        private void AddUserMessage(string message)
        {
            Paragraph paragraph =
                new Paragraph();

            paragraph.TextAlignment =
                TextAlignment.Right; 

            paragraph.Margin =
                new Thickness(10);

            Border border =
                new Border();

            border.Background =
                Brushes.DarkCyan;

            border.CornerRadius =
                new CornerRadius(10);

            border.Padding =
                new Thickness(10);

            TextBlock text =
                new TextBlock();

            text.Text =
                "You: " + message;

            text.Foreground =
                Brushes.White;

            text.TextWrapping =
                TextWrapping.Wrap;

            border.Child = text;

            InlineUIContainer container =
                new InlineUIContainer(border);

            paragraph.Inlines.Add(container);

            rtbChat.Document.Blocks.Add(paragraph);

            rtbChat.ScrollToEnd();
        }
    }
}