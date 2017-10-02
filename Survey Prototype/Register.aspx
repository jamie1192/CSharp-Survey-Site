<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Survey_Prototype.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    
<link href="Stylesheet.css" rel="stylesheet" type="text/css" />

        <div class="bodyContainer">
            <div class="bodyTitle">
               Registration
            </div>

            <span class="givenNames titleStyle">Given Names</span>
            <br />
            <br />
            
            <!-- Given Names fields -->
            <asp:TextBox ID="firstNameTextBox" runat="server" CssClass="TextBoxInputs" placeholder=" First Name"></asp:TextBox>
            <br />
            <br />

        <%--    <asp:TextBox ID="middleNameTextBox" runat="server" CssClass="TextBoxInputs" placeholder=" Middle name (optional)"></asp:TextBox>
            <br />
            <br />--%>

            <asp:TextBox ID="lastNameTextBox" runat="server" CssClass="TextBoxInputs" placeholder=" Last Name"></asp:TextBox>
            <br />
            <br />

            <!-- DOB Container -->
            <div class="DOBContainer">
                <span class="titleStyle">Date of Birth</span>
                <br />
                <br />

                <asp:TextBox ID="DOBDayTextBox" runat="server" CssClass="DOBTextBoxes" placeholder=" DD"></asp:TextBox>
               

                <asp:TextBox ID="DOBMonthTextBox" runat="server" CssClass="DOBTextBoxes" placeholder=" MM"></asp:TextBox>
        

                <asp:TextBox ID="DOBYearTextBox" runat="server" CssClass="DOBTextBoxes" placeholder=" YYYY"></asp:TextBox>
        
            </div>

            <!-- Phone Container -->
            <div class="phoneContainer">
                <span class="titleStyle">Contact Phone Number</span>
                <br />
                <br />

                <asp:TextBox ID="phoneNumberTextBox" runat="server" CssClass="TextBoxInputs" placeholder=" xxxx-xxx-xxx"></asp:TextBox>

            </div>

            <asp:Button ID="submitRegistrationButton" runat="server" OnClick="SubmitRegistration" Text="Submit Registration" CssClass="submitButton" />

        </div>

        
    <!-- </form> -->


</asp:Content>
