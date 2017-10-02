using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey_Prototype
{
    public class AppSession
    {
        public static void setUsername(string username) //set username after successful login
        {
            HttpContext.Current.Session["staffUserName"] = username;
        }

        public static string getUserName() //get logged in staff member username
        {
            return (string)HttpContext.Current.Session["staffUserName"];
        }

        public static bool IsLoggedIn() //check if user is logged in
        {
            string username = getUserName();
            if (username != null && username.Length > 0)
                return true;
            else
                return false;
        }
    }
}