using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Survey_Prototype
{
    public partial class StaffSearch : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

            SqlConnection connection = ConnectToDatabase();

            if (!AppSession.IsLoggedIn()) //redirect to login page if no user is logged in
            {
                Response.Redirect("~/StaffLogin.aspx");
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("UID");
            dt.Columns.Add("First Name");
            dt.Columns.Add("Last Name");
            dt.Columns.Add("Phone Number");
            
            DataRow myRow;

            string loadUsers = "SELECT * FROM userTable;";

            SqlCommand getUsersQuery = new SqlCommand(loadUsers, connection);

            SqlDataReader reader = getUsersQuery.ExecuteReader();

            while (reader.Read()) //display all surveyed users
            {
                myRow = dt.NewRow();
                myRow["UID"] = reader["u_Id"].ToString();
                myRow["First Name"] = reader["firstName"].ToString();
                myRow["Last Name"] = reader["lastName"].ToString();
                myRow["Phone Number"] = reader["phoneNumber"].ToString();   

                dt.Rows.Add(myRow);
            }

            searchResultsGridView.DataSource = dt;
            searchResultsGridView.DataBind();
            connection.Close();
        }

        protected void SearchResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void Search(object sender, EventArgs e)
        {
            SqlConnection connection = ConnectToDatabase();

            string staffSearchQuery = "SELECT * FROM userTable";

            List<ListItem> selected = new List<ListItem>(); //Gather all selected checkboxes
            foreach (ListItem item in bankCheckBoxList.Items) //Selected bank names to search for
            {
                if (item.Selected)
                {
                    selected.Add(item); //Add bank to search
                }
            }
            foreach (ListItem item in bankServiceCheckBoxList.Items) //Selected bank services to search for
            {
                if (item.Selected)
                {
                    selected.Add(item);
                }
            }
            foreach (ListItem item in genderCheckBoxList.Items) //Selected gender to search for
            {
                if (item.Selected)
                {
                    selected.Add(item);
                }
            }

            if (selected != null) //search criteria selected
            {
                staffSearchQuery += " WHERE u_Id IN (SELECT u_Id from userAnswersTable WHERE ";

                int selectedCount = 0; //keep count of how many search parameters to add to reference for SQL parameters

                for (int i = 0; i < selected.Count; i++)
                {
                    if (selectedCount > 0)
                    {
                        staffSearchQuery += " OR a_Id=@a_Id" + i; //additional search criteria
                    }
                    else
                    {
                        staffSearchQuery += "a_Id=@a_Id" + i; //first search criteria
                    }
                    selectedCount++;
                }

                staffSearchQuery += ");";
                SqlCommand search = new SqlCommand(staffSearchQuery, connection);

                for (int i = 0; i < selectedCount; i++)
                {
                    //Append the a_Id values to the query parameters to prevent SQL injection
                    search.Parameters.Add(new SqlParameter("a_Id" + i, selected[i].Value.ToString()));
                }

                try
                {
                    SqlDataReader reader = search.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("UID");
                    dt.Columns.Add("First Name");
                    dt.Columns.Add("Last Name");
                    dt.Columns.Add("Phone Number");

                    DataRow myRow;

                    while (reader.Read())
                    {
                        myRow = dt.NewRow();
                        myRow["UID"] = reader["u_Id"].ToString();
                        myRow["First Name"] = reader["firstName"].ToString();
                        myRow["Last Name"] = reader["lastName"].ToString();
                        myRow["Phone Number"] = reader["phoneNumber"].ToString();

                        dt.Rows.Add(myRow);
                    }

                    searchResultsGridView.DataSource = dt;
                    searchResultsGridView.DataBind();
                }
                catch(Exception err)
                {
                    System.Console.Write("Database/connection error: " + err);
                }
                connection.Close();
            }

            else //no search criteria selected, just return all respondants
            {
                SqlCommand search = new SqlCommand(staffSearchQuery, connection);

                try
                {
                    SqlDataReader reader = search.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("UID");
                    dt.Columns.Add("First Name");
                    dt.Columns.Add("Last Name");
                    dt.Columns.Add("Phone Number");

                    DataRow myRow;

                    while (reader.Read())
                    {
                        myRow = dt.NewRow();
                        myRow["UID"] = reader["u_Id"].ToString();
                        myRow["First Name"] = reader["firstName"].ToString();
                        myRow["Last Name"] = reader["lastName"].ToString();
                        myRow["Phone Number"] = reader["phoneNumber"].ToString();

                        dt.Rows.Add(myRow);
                    }

                    searchResultsGridView.DataSource = dt;
                    searchResultsGridView.DataBind();
                }
                catch(Exception err)
                {
                    System.Console.Write("Database/connection error: " + err);
                }
                connection.Close();
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
