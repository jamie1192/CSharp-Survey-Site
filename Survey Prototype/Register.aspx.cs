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

            string ipAddress = GetIPAddress();
            //string cmd = "INSERT INTO testTable (ipAddress) VALUES ('" + ipAddress + "');SELECT CAST(scope_identity() AS int)";
            //string cmd = "INSERT INTO testTable (ipAddress) VALUES ('"+ipAddress+"')";

            //SqlCommand testUser = new SqlCommand(cmd, connection);
            //SqlCommand testUser = new SqlCommand("INSERT INTO testTable (ipAddress) VALUES ('" + ipAddress + "')" + connection);
            //int newID = (int)testUser.ExecuteScalar();
            //connection.Close();
        }

        protected void SubmitRegistration(object sender, EventArgs e)
        {
            //String textAnswer = ch.QuestionTextBox.Text;
            //string firstName = ((TextBox)MainContent.FindControl("firstNameTextBox")).Text;
            string firstName = firstNameTextBox.Text;
            string middleName = middleNameTextBox.Text;
            string lastName = lastNameTextBox.Text;

            SqlConnection connection = ConnectToDatabase();

            //save user data to DB
            //get generated u_Id to throw into saveData below
            string ipAddress = GetIPAddress();
            int user_Id = 0;

            List<questionData> getAnswers = (List<questionData>)HttpContext.Current.Session["userAnswers"];

            for(int i = 0; i < getAnswers.Count; i++)
            {
                string qID = getAnswers[i].q_Id;
                string aID = getAnswers[i].a_Id;
                string text = getAnswers[i].text;

                SqlCommand saveData = new SqlCommand("INSERT INTO userAnswersTable (u_Id, a_Id, answerText) VALUES ("+ user_Id +","+ aID + "," + "'" + text + "')" + connection);
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

        protected string GetIPAddress()
        {
            //get IP through PROXY
            //====================
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //should break ipAddress down, but here is what it looks like:
            // return ipAddress;
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] address = ipAddress.Split(',');
                if (address.Length != 0)
                {
                    return address[0];
                }
            }
            //if not proxy, get nice ip, give that back :(
            //ACROSS WEB HTTP REQUEST
            //=======================
            ipAddress = context.Request.UserHostAddress;//ServerVariables["REMOTE_ADDR"];

            if (ipAddress.Trim() == "::1")//ITS LOCAL(either lan or on same machine), CHECK LAN IP INSTEAD
            {
                //This is for Local(LAN) Connected ID Address
                string stringHostName = System.Net.Dns.GetHostName();
                //Get Ip Host Entry
                System.Net.IPHostEntry ipHostEntries = System.Net.Dns.GetHostEntry(stringHostName);
                //Get Ip Address From The Ip Host Entry Address List
                System.Net.IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                try
                {
                    ipAddress = arrIpAddress[1].ToString();
                }
                catch
                {
                    try
                    {
                        ipAddress = arrIpAddress[0].ToString();
                    }
                    catch
                    {
                        try
                        {
                            arrIpAddress = System.Net.Dns.GetHostAddresses(stringHostName);
                            ipAddress = arrIpAddress[0].ToString();
                        }
                        catch
                        {
                            ipAddress = "127.0.0.1";
                        }
                    }
                }
            }
            return ipAddress;
        }


    }
}