using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey_Prototype
{
    public class AppSession
    {
        //Staff member
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

        //User/Survey responder
        public static void setResponderUserId(int uid)
        {
            HttpContext.Current.Session["u_Id"] = uid;
        }

        public static int getResponderUserId()
        {
            return (int)HttpContext.Current.Session["u_Id"];
        }

        //Ensure same user can't do survey again
        public static void setSurveyCompleted(bool completed)
        {
            HttpContext.Current.Session["surveyCompleted"] = completed;
        }

        public static bool getSurveyCompletedStatus()
        {
            if (HttpContext.Current.Session["surveyCompleted"] != null)
            {
                return (bool)HttpContext.Current.Session["surveyCompleted"];
            }
            else
                return false;
        }
    }
}