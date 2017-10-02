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
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            SqlConnection connection = ConnectToDatabase();

            for (int day = 1; day <= 31; day++)
            {
                dobDay.Items.Add(day.ToString());
            }
            for (int month = 1; month <= 12; month++)
            {
                dobMonth.Items.Add(month.ToString());
            }
            for (int year = 1900; year <= (Convert.ToInt32(DateTime.Now.Year.ToString())); year++)
            {
                dobYear.Items.Add(year.ToString());
            }
        }

        protected void SubmitRegistration(object sender, EventArgs e)
        {
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string phoneNumber = phoneNumberTextBox.Text;
            string dobConcatenate = dobDay.SelectedItem.ToString() + "/" + dobMonth.SelectedItem.ToString() + "/" + dobYear.SelectedItem.ToString();


            SqlConnection connection = ConnectToDatabase();

            string registerUserQuery = "UPDATE userTable SET firstName=@firstName, lastname=@lastName, phoneNumber=@phone, dob=@dob WHERE u_Id=@u_Id;";

            SqlCommand registerUser = new SqlCommand(registerUserQuery, connection);
            registerUser.Parameters.Add(new SqlParameter("firstName", firstName));
            registerUser.Parameters.Add(new SqlParameter("lastName", lastName));
            registerUser.Parameters.Add(new SqlParameter("phone", phoneNumber));
            registerUser.Parameters.Add(new SqlParameter("dob", dobConcatenate));
            registerUser.Parameters.Add(new SqlParameter("u_Id", AppSession.getResponderUserId()));

            try
            {
                registerUser.ExecuteNonQuery();
                connection.Close();

                AppSession.setSurveyCompleted(true);
                Response.Redirect("~/SurveyCompleted.aspx");
            }
            catch (Exception err)
            {
                Console.Write("Database/connection error: " + err);
            }

        }


        private static SqlConnection ConnectToDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Database"].ToString();
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = connectionString
            };
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