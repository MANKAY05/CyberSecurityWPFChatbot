using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace CyberSecurityWPFChatbot
{
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            InitializeComponent();

            LoadHistory();
        }

        private void LoadHistory()
        {
            try
            {
                string connectionString =
     "server=localhost;" +
     "database=CyberBotDB;" +
     "uid=root;" +
      "pwd=mankay;";

                using (MySqlConnection con =
                       new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query =
                        "SELECT * FROM ChatHistory";

                    MySqlDataAdapter adapter =
                        new MySqlDataAdapter(query, con);

                    DataTable table =
                        new DataTable();

                    adapter.Fill(table);

                    dgHistory.ItemsSource =
                        table.DefaultView;
                }
            }
            catch
            {
                MessageBox.Show(
                    "Unable to load chat history.");
            }
        }
    }
}