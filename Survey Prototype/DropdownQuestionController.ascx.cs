using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Survey_Prototype
{
    public partial class DropdownQuestionController : System.Web.UI.UserControl
    {

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

        public DropDownList DropdownQuestionList
        {
            get
            {
                return dropdownQuestionList;
            }
            set
            {
                dropdownQuestionList = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}