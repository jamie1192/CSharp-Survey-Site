using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Survey_Prototype
{
    public partial class StaffLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = staffUsernameText.Text;
            string password = staffPasswordText.Text;

            SqlConnection connection = ConnectToDatabase();

            string findUserQueryString = "SELECT * FROM staffUsersTable WHERE username=@username AND password=@password;";

            SqlCommand findUser = new SqlCommand(findUserQueryString, connection);
            findUser.Parameters.Add(new SqlParameter("username", username));
            findUser.Parameters.Add(new SqlParameter("password", password));

            SqlDataReader reader = findUser.ExecuteReader();

            if (reader.Read())
            {
                string staffUsername = reader["username"].ToString();
                AppSession.setUsername(staffUsername);

                Response.Redirect("~/StaffSearch.aspx");
            }
            else{
                loginErrorMessage.Text = "Username and/or password incorrect!";
            }
        }


        private static SqlConnection ConnectToDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Database"].ToString();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.Write("I probably dun' goofed: " + e);
            }

            return connection;
        }
    }
}