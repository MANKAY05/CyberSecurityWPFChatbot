using MySql.Data.MySqlClient;
using System;

public class DatabaseHelper
{
    private string connectionString =
        "server=localhost;database=CyberBotDB;uid=root;pwd=mankay;";

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }      

    public void SaveActivity(string activity)
    {
        using (MySqlConnection con = GetConnection())
        {
            con.Open();

            string query = @"
                INSERT INTO ActivityLog (Activity, LogDate)
                VALUES (@activity, NOW())";

            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@activity", activity);
                cmd.ExecuteNonQuery();
            }
        }
    }
    public void SaveChat(
    string userMessage,
    string botResponse)
    {
        // Your MySQL insert code here
    }
}