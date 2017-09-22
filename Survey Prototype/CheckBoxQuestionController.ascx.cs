using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Survey_Prototype
{
    public partial class CheckBoxQuestionController : System.Web.UI.UserControl
    {
        //Getters and setters
        public Label QuestionLabel
        {
            get
            {
                return questionLabel;
            }
            set
            {
                questionLabel = value;
            }
        }


        public CheckBoxList QuestionCheckBoxList
        {
            get
            {
                return questionCheckBoxList;
            }
            set
            {
                questionCheckBoxList = value;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}