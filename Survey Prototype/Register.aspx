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
            <div class="nameContainer">
                <div class="nameInputContainer">
                    <asp:TextBox ID="firstNameTextBox" runat="server" CssClass="TextBoxInputs" placeholder=" First Name"></asp:TextBox>
                </div>
                <div class="nameErrorContainer">
                    <asp:RegularExpressionValidator ID="firstNameValidator" runat="server" CssClass="inputErrorMessage"
                                                        ErrorMessage="Invalid characters."
                                                        ControlToValidate="firstNameTextBox"
                                                        ValidationExpression="^([ \u00c0-\u01ffa-zA-Z'\-])+$"></asp:RegularExpressionValidator>
                </div>
            </div>

            

        <%--    <asp:TextBox ID="middleNameTextBox" runat="server" CssClass="TextBoxInputs" placeholder=" Middle name (optional)"></asp:TextBox>
            <br />
            <br />--%>

            <div class="nameContainer">
                <div class="nameInputContainer">
                    <asp:TextBox ID="lastNameTextBox" runat="server" CssClass="TextBoxInputs" placeholder=" Last Name"></asp:TextBox>
                </div>
                    <div class="nameErrorContainer">
                        <asp:RegularExpressionValidator ID="lastNameValidator" runat="server" CssClass="inputErrorMessage"
                                                            ErrorMessage="Invalid characters."
                                                            ControlToValidate="lastNameTextBox"
                                                            ValidationExpression="^([ \u00c0-\u01ffa-zA-Z'\-])+$"></asp:RegularExpressionValidator>
                    </div>
            </div>
            

            <!-- DOB Container -->
            <div class="DOBContainer">
                <span class="titleStyle">Date of Birth</span>
                <br />
                <br />

                <%--<asp:TextBox ID="DOBDayTextBox" runat="server" TextMode="Date" CssClass="DOBTextBoxes" placeholder=" DD"></asp:TextBox>--%>
                <div class="dobDropdowns">
                    <asp:DropDownList ID="dobDay" runat="server"></asp:DropDownList>

                    <%--<asp:TextBox ID="DOBMonthTextBox" runat="server" CssClass="DOBTextBoxes" placeholder=" MM"></asp:TextBox>--%>
                    <asp:DropDownList ID="dobMonth" runat="server"></asp:DropDownList>

                    <%--<asp:TextBox ID="DOBYearTextBox" runat="server" CssClass="DOBTextBoxes" placeholder=" YYYY"></asp:TextBox>--%>
                    <asp:DropDownList ID="dobYear" runat="server"></asp:DropDownList>
                </div>
            </div>

            <!-- Phone Container -->
            <div class="phoneContainer">
                <span class="titleStyle">Contact Phone Number</span>
                <br />
                <br />

                <div class="phoneTextBoxContainer">
                    <asp:TextBox ID="phoneNumberTextBox" runat="server" TextMode="Phone" CssClass="" placeholder="Home/Mobile"></asp:TextBox>
                </div>
                <div class="phoneErrorContainer">
                    <asp:RegularExpressionValidator ID="phoneRegex" runat="server" CssClass="inputErrorMessage"
                                                    ErrorMessage="Not a valid phone number format."
                                                    ControlToValidate="phoneNumberTextBox"
                                                    ValidationExpression="([0-9]{10})|([0-9]{8})"></asp:RegularExpressionValidator>
                </div>
            </div>

            <asp:Button ID="submitRegistrationButton" runat="server" OnClick="SubmitRegistration" Text="Submit/Skip Registration" CssClass="submitButton" />

        </div>

        
    <!-- </form> -->


</asp:Content>
