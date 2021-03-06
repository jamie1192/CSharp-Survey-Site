﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Survey_Prototype
{
    public partial class Question : System.Web.UI.Page
    {
        CheckBoxList debugList = new CheckBoxList();
        int questionType;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.getSurveyCompletedStatus())
            {
                Response.Redirect("~/SurveyCompleted.aspx");
            }

            int currentQuestion = GetCurrentQuestionNumber();
            SqlConnection connection = ConnectToDatabase();
            int nextQuestion = 0;

            //load question from database
            SqlCommand getQuestion = new SqlCommand("SELECT * FROM questionTable WHERE questionTable.q_Id = " + currentQuestion, connection);

            //run query
            SqlDataReader reader = getQuestion.ExecuteReader();

            while (reader.Read())
            {
                //read questions returned from query
                string questionText = reader["questionText"].ToString();
                questionType = Convert.ToInt32(reader["questionType"]);
                List<String> checkForFollowUps = new List<string>();
                checkForFollowUps = (List<string>)HttpContext.Current.Session["followUpQuestions"];

                    
                if (reader["nextQuestion_Id"] != DBNull.Value) //gets next question if it exists
                {
                    nextQuestion = Convert.ToInt32(reader["nextQuestion_Id"]);
                    HttpContext.Current.Session["questionNumberTemp"] = nextQuestion;
                }
                else //no more standard questions left
                {
                    HttpContext.Current.Session["surveyProgress"] = 1;
                }

                    //load the appropriate questionController
                    if (questionType == 1) //textbox
                    {

                        //load TexQuestionController
                        TextQuestionController textController = (TextQuestionController)LoadControl("~/TextQuestionController.ascx");

                        //set the ID to reference to it later
                        textController.ID = "TextQuestionController";
                    
                        //Insert the active question into label
                        textController.QuestionLabel.Text = questionText;

                        //Insert controller to the placeholder
                        QuestionPlaceholder.Controls.Add(textController);
                    }

                    else if (questionType == 2) //checkbox 
                    {
                        CheckBoxQuestionController checkBoxController = (CheckBoxQuestionController)LoadControl("~/CheckBoxQuestionController.ascx");

                        checkBoxController.ID = "checkBoxQuestionController";
                        checkBoxController.QuestionLabel.Text = questionText;
                        string followUpID;

                        SqlCommand optionCommand = new SqlCommand("SELECT * FROM answerOptionTable WHERE answerOptionTable.q_Id = " + currentQuestion, connection);

                        //run command
                        try
                        {
                            SqlDataReader optionReader = optionCommand.ExecuteReader();

                            //loop through all results
                            while (optionReader.Read())
                            {
                                ListItem item = new ListItem(optionReader["answerText"].ToString(), optionReader["a_Id"].ToString());
 
                                if (optionReader["fq_Id"] != DBNull.Value)
                                {
                                    string currentA_Id = optionReader["a_Id"].ToString();
                                    followUpID = optionReader["fq_Id"].ToString();
                                    HttpContext.Current.Session[currentA_Id] = followUpID; //filthy workaround to store fq_Id's by its answer_Id
                                }

                                int currentAnswerId = Convert.ToInt32(optionReader["a_Id"]);

                                checkBoxController.QuestionCheckBoxList.Items.Add(item); //add answer to list
                            }
                            QuestionPlaceholder.Controls.Add(checkBoxController); //add all answers to placeholder
                        }
                        catch (Exception err)
                        {
                            Console.Write("Database/connection error" + err);
                        }
                    }

                    else if (questionType == 3) // dropdown
                    {
                        DropdownQuestionController dropdownController = (DropdownQuestionController)LoadControl("~/DropdownQuestionController.ascx");

                        dropdownController.ID = "dropdownQuestionController";
                        dropdownController.QuestionLabel.Text = questionText;

                        SqlCommand optionCommand = new SqlCommand("SELECT * FROM answerOptionTable WHERE answerOptionTable.q_Id = " + currentQuestion, connection);

                        //run command
                        try
                        {
                            SqlDataReader optionReader = optionCommand.ExecuteReader();

                            //loop through all results
                            while (optionReader.Read())
                            {
                                ListItem item = new ListItem(optionReader["answerText"].ToString(), optionReader["a_Id"].ToString());
                                dropdownController.DropdownQuestionList.Items.Add(item); //add answer to list
                            }

                            //add all retrieved answers to controller
                            QuestionPlaceholder.Controls.Add(dropdownController);
                        }
                        catch (Exception err)
                        {
                            Console.Write("Database/connection error" + err);
                        }
                    }

                    else if (questionType == 4) //radio 
                    {
                        RadioQuestionController radioController = (RadioQuestionController)LoadControl("~/RadioQuestionController.ascx");

                        radioController.ID = "radioQuestionController";
                        radioController.QuestionLabel.Text = questionText;

                        SqlCommand optionCommand = new SqlCommand("SELECT * FROM answerOptionTable WHERE answerOptionTable.q_Id = " + currentQuestion, connection);

                        //run command
                        try
                        {
                            SqlDataReader optionReader = optionCommand.ExecuteReader();

                            //loop through all results
                            while (optionReader.Read())
                            {
                                ListItem item = new ListItem(optionReader["answerText"].ToString(), optionReader["a_Id"].ToString());
                                radioController.RadioQuestionList.Items.Add(item);
                            }
                            QuestionPlaceholder.Controls.Add(radioController);
                        }
                        catch (Exception err)
                        {
                            Console.Write("Database/connection error: " + err);
                        }
                    }
           
            }
            connection.Close();
        }

        private static int GetCurrentQuestionNumber()
        {
            int currentQuestion = 0; 

            if (HttpContext.Current.Session["questionNumber"] == null)//start of survey
            {
                SqlConnection connection = ConnectToDatabase();
                SqlCommand getFirstQuestion = new SqlCommand("SELECT TOP 1 * FROM questionTable; ", connection);
                
                try
                {
                    SqlDataReader reader = getFirstQuestion.ExecuteReader();
                    while (reader.Read())
                    {
                        currentQuestion = Convert.ToInt32(reader["q_Id"]);
                        HttpContext.Current.Session["questionNumber"] = currentQuestion; //save question to session
                    }
                }
                catch (Exception err)
                {
                    Console.Write("Database/connection error: " + err);
                }
                connection.Close();
            }
            
            else //retrieve survey progress from session
            {
                int nextQuestion = (int)HttpContext.Current.Session["questionNumber"]; //get next question that was set when question was generated   
                currentQuestion = nextQuestion;
            }
            return currentQuestion;
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

        protected void SubmitButtonClick(object sender, EventArgs e)
        {
            List<questionData> storedAnswers = new List<questionData>();
            List<String> followUpQuestionList = new List<string>();

            if (questionType == 1) //text input
            {
                TextQuestionController ch = (TextQuestionController)QuestionPlaceholder.FindControl("TextQuestionController");

                int currentQuestion = (int)HttpContext.Current.Session["questionNumber"];

                if (HttpContext.Current.Session["userAnswers"] != null) //if there are saved answers from previous q's
                {
                    storedAnswers = (List<questionData>)HttpContext.Current.Session["userAnswers"];
                }

                if (ch != null)
                {
                    String textAnswer = ch.QuestionTextBox.Text;

                    questionData saveQuestion = new questionData //instantiate new questionData class to save as listItem
                    {
                        q_Id = currentQuestion.ToString(),
                        a_Id = "",
                        text = textAnswer
                    };

                    storedAnswers.Add(saveQuestion);
                    HttpContext.Current.Session["userAnswers"] = storedAnswers;
                }
            }

            else if(questionType == 2) //checkbox
            {
                CheckBoxQuestionController ch = (CheckBoxQuestionController)QuestionPlaceholder.FindControl("checkBoxQuestionController");
                string value;
                int currentQuestion = (int)HttpContext.Current.Session["questionNumber"];

                if (HttpContext.Current.Session["userAnswers"] != null) //if there are saved answers from previous q's
                {
                    storedAnswers = (List<questionData>)HttpContext.Current.Session["userAnswers"];
                }
                if (HttpContext.Current.Session["followUpQuestions"] != null) //if there's already follow up questions ready to go
                {
                    followUpQuestionList = (List<string>)HttpContext.Current.Session["followUpQuestions"];
                }
                    
                if (ch != null)
                {
                    foreach (ListItem chk in ch.QuestionCheckBoxList.Items) 
                    {
                        if (chk.Selected) //if checkbox was selected by user
                        {
                            value = chk.Value; //gets the a_Id

                            if(HttpContext.Current.Session[chk.Value] != null) //if there's a follow up question associated with this answer
                            {
                                if (followUpQuestionList.Count > 0) //if there's already a queue of followUps waiting
                                {
                                    if(followUpQuestionList[0] != HttpContext.Current.Session[chk.Value].ToString()) //if the followUpQuestion isn't already queued (don't duplicate question)
                                        followUpQuestionList.Add(HttpContext.Current.Session[chk.Value].ToString());
                                }
                                else 
                                {
                                    followUpQuestionList.Add(HttpContext.Current.Session[chk.Value].ToString()); //add new followUp
                                }
                            }

                            questionData saveQuestion = new questionData //instantiate new questionData class to save as listItem
                            {
                                q_Id = currentQuestion.ToString(),
                                a_Id = chk.Value,
                                text = ""
                            };

                            storedAnswers.Add(saveQuestion);
                        }
                    }
                    HttpContext.Current.Session["userAnswers"] = storedAnswers; //append stored answers session
                    HttpContext.Current.Session["followUpQuestions"] = followUpQuestionList; //append followUp list
                }
            }

            else if(questionType == 3) //dropdown
            {
                DropdownQuestionController ch = (DropdownQuestionController)QuestionPlaceholder.FindControl("dropdownQuestionController");

                int currentQuestion = (int)HttpContext.Current.Session["questionNumber"];

                if (HttpContext.Current.Session["userAnswers"] != null) //if there are saved answers from previous q's
                {
                    storedAnswers = (List<questionData>)HttpContext.Current.Session["userAnswers"];
                }

                if (ch != null)
                {
                    foreach (ListItem chk in ch.DropdownQuestionList.Items)
                    {
                        if (chk.Selected)
                        {
                            string value = chk.Value; //gets the answer_Id

                            questionData saveQuestion = new questionData
                            {
                                q_Id = currentQuestion.ToString(),
                                a_Id = chk.Value,
                                text = ""
                            };
                            storedAnswers.Add(saveQuestion); //push onto saved answers list
                        }
                    }
                    HttpContext.Current.Session["userAnswers"] = storedAnswers;
                }
            }

            else if(questionType == 4) //radio
            {
                RadioQuestionController ch = (RadioQuestionController)QuestionPlaceholder.FindControl("radioQuestionController");

                string value;
                int currentQuestion = (int)HttpContext.Current.Session["questionNumber"];

                if (HttpContext.Current.Session["userAnswers"] != null) //if there are saved answers from previous q's
                {
                    storedAnswers = (List<questionData>)HttpContext.Current.Session["userAnswers"];
                }

                if (ch != null)
                {
                    foreach (ListItem chk in ch.RadioQuestionList.Items)
                    {
                        if (chk.Selected)
                        {
                            value = chk.Value; //gets the a_Id

                            questionData saveQuestion = new questionData();
                            saveQuestion.q_Id = currentQuestion.ToString();
                            saveQuestion.a_Id = chk.Value;
                            saveQuestion.text = "";

                            storedAnswers.Add(saveQuestion); //save users answer
                        }
                    }
                    HttpContext.Current.Session["userAnswers"] = storedAnswers; //append saved answers list in session
                }
            }


            if((followUpQuestionList.Count == 0) && ((int)HttpContext.Current.Session["questionNumberTemp"] == 0)) //if no followUps and no next question - end of survey
            {
                SqlConnection connection = ConnectToDatabase();
                string ipAddress = GetIPAddress();
                
                //create user and get u_Id
                string cmd = "INSERT INTO userTable (ipAddress) VALUES ('" + ipAddress + "');SELECT CAST(scope_identity() AS int)";
                
                SqlCommand insertUser = new SqlCommand(cmd, connection);
                
                int newUser_Id = (int)insertUser.ExecuteScalar(); //get newly created u_Id
                
                AppSession.setResponderUserId(newUser_Id); //store in session

                //save answers from survey
                List<questionData> getAnswers = (List<questionData>)HttpContext.Current.Session["userAnswers"];

                for (int i = 0; i < getAnswers.Count; i++)
                {
                    string qID = getAnswers[i].q_Id;
                    string aID = getAnswers[i].a_Id;
                    string text = getAnswers[i].text;
                    //save answer to each question
                    try
                    {
                        string s = "INSERT INTO userAnswersTable (u_Id, a_Id, answerText) VALUES ('" + newUser_Id + "','" + aID + "'," + "'" + text + "')";
                        SqlCommand saveData = new SqlCommand("INSERT INTO userAnswersTable (u_Id, a_Id, answerText) VALUES ('" + newUser_Id + "','" + aID + "'," + "'" + text + "')", connection);

                        //run query
                        saveData.ExecuteNonQuery();
                    }
                    catch(Exception err)
                    {
                        Console.Write("Database/network error occurred: " + err);
                    }
                    
                }
                connection.Close();
                Response.Redirect("Register.aspx"); //redirect to register page
            }

            else if (followUpQuestionList.Count == 0) //next normal question
            {
                int nextQuestion = (int)HttpContext.Current.Session["questionNumberTemp"]; //get next question that was set when question was generated   
                HttpContext.Current.Session["questionNumber"] = nextQuestion;
                HttpContext.Current.Session["questionNumberTemp"] = 0;

                Response.Redirect("Question.aspx");
            }

            else //get queued followUps
            {
                List<string> followUpList = (List<string>)HttpContext.Current.Session["followUpQuestions"];
                if (followUpList.Count > 0)
                {
                    int nextQuestion = Convert.ToInt32(followUpList[0]); //set next question as the first queued follow up
                    HttpContext.Current.Session["questionNumber"] = nextQuestion; //store it in the session
                    followUpList.RemoveAt(0); //remove this follow up from the queue
                    HttpContext.Current.Session["followUpQuestions"] = followUpList; //append the session-stored followUp list 
                }
                Response.Redirect("Question.aspx");
            }
        }



        protected string GetIPAddress()
        {
            //get IP through PROXY
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            // return ipAddress;
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] address = ipAddress.Split(',');
                if (address.Length != 0)
                {
                    return address[0];
                }
            }
            
            //ACROSS WEB HTTP REQUEST
            ipAddress = context.Request.UserHostAddress;

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