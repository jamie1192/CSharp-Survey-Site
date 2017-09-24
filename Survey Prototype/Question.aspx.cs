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
    public partial class Question : System.Web.UI.Page
    {
        CheckBoxList debugList = new CheckBoxList();

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (IsPostBack) //trying various ways to get survey answers within postback but can't manage to pinpoint
                            //where each checkbox is within it's parent CheckBoxList..
            {
                //template.Items.
                Control resultControl = FindControl("checkBoxQuestionController");

                //test 1
                CheckBoxList resultControl2 = (CheckBoxList)FindControl("checkBoxQuestionController");

                //CheckBoxList resultControl3 = (CheckBoxList)FindControl("questionCheckBoxList");
                CheckBoxList findList = Form.FindControl("checkBoxQuestionController") as CheckBoxList;

                //test 123213
                CheckBoxList Cbx = (CheckBoxList)QuestionPlaceholder.FindControl("checkBoxQuestionController");

               foreach(ListItem li in QuestionPlaceholder.Controls)
                {
                    var value = li.Value;
                }

                String cbList2 = Request.Form["questionCheckBoxList"];

            }

            int currentQuestion = GetCurrentQuestionNumber();
            SqlConnection connection = ConnectToDatabase();
            int nextQuestion = 0;

            if (HttpContext.Current.Session["tempCheckbox"] != null)
            {
                debugList = (CheckBoxList)HttpContext.Current.Session["tempCheckbox"];
            }

                //load question from database
                SqlCommand getQuestion = new SqlCommand("SELECT * FROM questionTable WHERE questionTable.q_Id = " + currentQuestion, connection);

                //run query
                SqlDataReader reader = getQuestion.ExecuteReader();


                while (reader.Read())
                {
                    //read questions returned from query
                    string questionText = reader["questionText"].ToString();
                    int questionType = Convert.ToInt32(reader["questionType"]);

                    //TODO (if reader["nextQuestion_Id"] == DBNull.Value)
                    //bool finishedSurvey = true;
                    nextQuestion = Convert.ToInt32(reader["nextQuestion_Id"]);
                //questionText += nextQuestion;               


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

                    SqlCommand optionCommand = new SqlCommand("SELECT * FROM answerOptionTable WHERE answerOptionTable.q_Id = " + currentQuestion, connection);

                    //run command
                    try
                    {
                        SqlDataReader optionReader = optionCommand.ExecuteReader();

                        //loop through all results
                        while (optionReader.Read())
                        {
                            //TODO if optionReader["fq_Id"] != DBNull.Value, create session list to store followUp
                            ListItem item = new ListItem(optionReader["answerText"].ToString(), optionReader["a_Id"].ToString());
                            //CheckBox cb = new QuestionCheckBoxList(optionReader)

                            int currentAnswerId = Convert.ToInt32(optionReader["a_Id"]);

                            checkBoxController.QuestionCheckBoxList.Items.Add(item); //add answer to list
                                                                                     //checkBoxController.QuestionCheckBoxList.Controls.Add(item);
                            debugList.Items.Add(item);
                        }
                        HttpContext.Current.Session["tempCheckbox"] = debugList;

                        QuestionPlaceholder.Controls.Add(checkBoxController);


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
                                //radioTemplate.Add(item);
                            }

                            QuestionPlaceholder.Controls.Add(radioController);
                        }
                        catch (Exception err)
                        {
                            Console.Write("Database/connection error: " + err);
                        }
                    }

                }

                HttpContext.Current.Session["questionNumber"] = nextQuestion;
                connection.Close();
            //}




        }

        private static int GetCurrentQuestionNumber()
        {
            //TODO FIX this
            int currentQuestion = 0; // old example answer

            //just some default value TODO: get start qNum from DB
            //check if question number stored in session
            if (HttpContext.Current.Session["questionNumber"] == null)//no?
            {
                SqlConnection connection = ConnectToDatabase();
                SqlCommand getFirstQuestion = new SqlCommand("SELECT TOP 1 * FROM questionTable; ", connection);
                Console.WriteLine(getFirstQuestion);
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
            //HttpContext.Current.Session["questionNumber"] = 1; //set for first time
            else //retrieve it from current session
            {
                currentQuestion = (int)HttpContext.Current.Session["questionNumber"];
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
            //template.Items.
            Control resultControl = FindControl("checkBoxQuestionController");

            //test 1
            CheckBoxList resultControl2 = (CheckBoxList)FindControl("checkBoxQuestionController");

            CheckBoxList resultControl3 = (CheckBoxList)FindControl("questionCheckBoxList"); 

            //test 123213
            CheckBoxList Cbx = (CheckBoxList)QuestionPlaceholder.FindControl("checkBoxQuestionController");

            //CheckBoxList Cb4 = (CheckBoxList)CheckBoxQuestionController.
            //foreach (ListItem li in (ListItem)resultControl)
            //{
            //    if (li.Selected)
            //    {

            //    }
            //}

            //test 2
            //for (int i = 0; i < QuestionPlaceholder.Controls.Count; i++)
            //{
            //    if (QuestionPlaceholder.Controls[i].GetType() == typeof(CheckBoxList))
            //    {
            //        CheckBoxList myList = QuestionPlaceholder.Controls[i].GetType();
            //    }
            //}

            //test 3
            //foreach (ListItem cbList in QuestionPlaceholder.Controls.("checkBoxQuestionController")
            //{
            //    if (cbList.Selected)
            //    {

            //    }
            //}
            //testc 4
            //foreach (ListItem cb in QuestionPlaceholder.Controls.OfType<ListItem>())
            //{
            //    if (cb != null)
            //    {

            //    }
            //}


                //Control searchControl1 = this.Controls[0].Controls[3].Controls[1].Controls[1].Controls[1];
                Response.Redirect("Question.aspx");

        }

        protected void SkipQuestion(object sender, EventArgs e)
        {

            //List<ListItem> selected = new List<ListItem>();
            //foreach (ListItem item in radioTemplate)
            //    if (item.Selected) selected.Add(item);
        }
    }
}