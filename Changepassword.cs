using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication13.Repository
{
    public class Changepassword
    {
        string conString = ConfigurationManager.ConnectionStrings["DBconnect"].ToString();

        public object YourPasswordHashingFunction { get; private set; }

        public bool ChangePassword(string username, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                string query = "UPDATE Signup SET Password = @NewPassword WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewPassword", newPassword);
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

        


    }
}