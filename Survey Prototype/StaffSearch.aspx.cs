using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Survey_Prototype
{
    public partial class StaffSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
            dt.Columns.Add("Email Address");

            DataRow myRow;

            //TODO while loop reader from DB
            myRow = dt.NewRow();
            myRow["UID"] = "1";
            myRow["First Name"] = "John";
            myRow["Last Name"] = "Smith";
            myRow["Phone Number"] = "04221234123";
            myRow["Email Address"] = "john123@gmail.com";

            dt.Rows.Add(myRow);

            searchResultsGridView.DataSource = dt;
            searchResultsGridView.DataBind();

        }

        protected void SearchResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void Search(object sender, EventArgs e)
        {

        }


    }
}
