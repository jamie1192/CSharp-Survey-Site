﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Survey_Prototype
{
    public partial class TextQuestionController : System.Web.UI.UserControl
    {
        public Label QuestionLabel
        {
            get
            {
                return questionLabel;
            }
            set
            {
                QuestionLabel = value;
            }
        }

        public TextBox QuestionTextBox
        {
            get
            {
                return questionTextBox;
            }
            set
            {
                questionTextBox = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}