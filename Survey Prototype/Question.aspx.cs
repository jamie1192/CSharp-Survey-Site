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
        int questionType;

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (IsPostBack) //trying various ways to get survey answers within postback but can't manage to pinpoint
                            //where each checkbox is within it's parent CheckBoxList..
            {

                string cbID = "questionCheckBoxList";
                //template.Items.

                CheckBoxList ch = (CheckBoxList)QuestionPlaceholder.FindControl("questionCheckBoxList");
                int count = 0;

                if (ch != null)
                {
                    foreach (ListItem chk in ch.Items)
                    {
                        if (chk.Selected)
                        {
                            //String str = chk.Text;
                            count++;
                        }
                    }
                }

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

            //if (HttpContext.Current.Session["tempCheckbox"] != null)
            //{
            //    debugList = (CheckBoxList)HttpContext.Current.Session["tempCheckbox"];
            //}

                //load question from database
                SqlCommand getQuestion = new SqlCommand("SELECT * FROM questionTable WHERE questionTable.q_Id = " + currentQuestion, connection);

                //run query
                SqlDataReader reader = getQuestion.ExecuteReader();


                while (reader.Read())
                {
                    //read questions returned from query
                    string questionText = reader["questionText"].ToString();
                    questionType = Convert.ToInt32(reader["questionType"]);

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
                            //debugList.Items.Add(item);
                        }
                        //HttpContext.Current.Session["tempCheckbox"] = debugList;

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

            HttpContext.Current.Session["questionNumberTemp"] = nextQuestion;
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
            
            if(questionType == 1) //text input
            {
                TextQuestionController ch = (TextQuestionController)QuestionPlaceholder.FindControl("TextQuestionController");

                string value;
                int currentQuestion = (int)HttpContext.Current.Session["questionNumber"];
                List<questionData> storedAnswers = new List<questionData>();

                //List<ListItem> testList = new List<ListItem>();

                if (HttpContext.Current.Session["userAnswers"] != null) //if there are saved answers from previous q's
                {
                    storedAnswers = (List<questionData>)HttpContext.Current.Session["userAnswers"];
                }
                else
                {
                    //List<questionData> storedAnswers = new List<questionData>();
                }

                if (ch != null)
                {
                    //foreach (ListItem chk in ch.RadioQuestionList.Items)
                    //{
                    //    if (chk.Selected)
                    //    {
                            //count++;
                    //        value = chk.Value; //gets the a_Id

                    //        questionData saveQuestion = new questionData();
                    //        saveQuestion.q_Id = currentQuestion;
                    //        saveQuestion.a_Id = Convert.ToInt32(chk.Value);
                    //        saveQuestion.text = "";

                    //        //ListItem item = new ListItem(currentQuestion.ToString(), chk.Value, "text");
                    //        storedAnswers.Add(saveQuestion);
                    //    //}
                    //}
                    String test = ch.QuestionTextBox.Text;
                    HttpContext.Current.Session["userAnswers"] = storedAnswers;
                }
            }

            else if(questionType == 2) //checkbox
            {
                CheckBoxQuestionController ch = (CheckBoxQuestionController)QuestionPlaceholder.FindControl("checkBoxQuestionController");
                int count = 0;
                string value;
                int currentQuestion = (int)HttpContext.Current.Session["questionNumber"];
                List<questionData> storedAnswers = new List<questionData>();

                //List<ListItem> testList = new List<ListItem>();

                if (HttpContext.Current.Session["userAnswers"] != null) //if there are saved answers from previous q's
                {
                    storedAnswers = (List<questionData>)HttpContext.Current.Session["userAnswers"];
                }
                else
                {
                    //List<questionData> storedAnswers = new List<questionData>();
                }
                    
                if (ch != null)
                {
                    foreach (ListItem chk in ch.QuestionCheckBoxList.Items)
                    {
                        if (chk.Selected)
                        {
                            count++;
                            value = chk.Value; //gets the a_Id

                            questionData saveQuestion = new questionData();
                            saveQuestion.q_Id = currentQuestion;
                            saveQuestion.a_Id = Convert.ToInt32(chk.Value);
                            saveQuestion.text = "";

                            //ListItem item = new ListItem(currentQuestion.ToString(), chk.Value, "text");
                            storedAnswers.Add(saveQuestion);
                        }
                    }
                    HttpContext.Current.Session["userAnswers"] = storedAnswers;
                }
            }

            else if(questionType == 3) //dropdown
            {
                DropdownQuestionController ch = (DropdownQuestionController)QuestionPlaceholder.FindControl("dropdownQuestionController");

                string value;
                int currentQuestion = (int)HttpContext.Current.Session["questionNumber"];
                List<questionData> storedAnswers = new List<questionData>();

                //List<ListItem> testList = new List<ListItem>();

                if (HttpContext.Current.Session["userAnswers"] != null) //if there are saved answers from previous q's
                {
                    storedAnswers = (List<questionData>)HttpContext.Current.Session["userAnswers"];
                }
                else
                {
                    //List<questionData> storedAnswers = new List<questionData>();
                }

                if (ch != null)
                {
                    foreach (ListItem chk in ch.DropdownQuestionList.Items)
                    {
                        if (chk.Selected)
                        {
                            //count++;
                            value = chk.Value; //gets the a_Id

                            questionData saveQuestion = new questionData();
                            saveQuestion.q_Id = currentQuestion;
                            saveQuestion.a_Id = Convert.ToInt32(chk.Value);
                            saveQuestion.text = "";

                            //ListItem item = new ListItem(currentQuestion.ToString(), chk.Value, "text");
                            storedAnswers.Add(saveQuestion);
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
                List<questionData> storedAnswers = new List<questionData>();

                //List<ListItem> testList = new List<ListItem>();

                if (HttpContext.Current.Session["userAnswers"] != null) //if there are saved answers from previous q's
                {
                    storedAnswers = (List<questionData>)HttpContext.Current.Session["userAnswers"];
                }
                else
                {
                    //List<questionData> storedAnswers = new List<questionData>();
                }

                if (ch != null)
                {
                    foreach (ListItem chk in ch.RadioQuestionList.Items)
                    {
                        if (chk.Selected)
                        {
                            //count++;
                            value = chk.Value; //gets the a_Id

                            questionData saveQuestion = new questionData();
                            saveQuestion.q_Id = currentQuestion;
                            saveQuestion.a_Id = Convert.ToInt32(chk.Value);
                            saveQuestion.text = "";

                            //ListItem item = new ListItem(currentQuestion.ToString(), chk.Value, "text");
                            storedAnswers.Add(saveQuestion);
                        }
                    }
                    HttpContext.Current.Session["userAnswers"] = storedAnswers;
                }

            }


            int nextQuestion = (int)HttpContext.Current.Session["questionNumberTemp"]; //get next question that was set when question was generated
            HttpContext.Current.Session["questionNumber"] = nextQuestion;
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