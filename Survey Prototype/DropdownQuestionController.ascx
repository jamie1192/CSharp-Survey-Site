<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropdownQuestionController.ascx.cs" Inherits="Survey_Prototype.DropdownQuestionController" %>

    <div class="bodyTitle">
        <asp:Label ID="questionLabel" runat="server" Text="LabelText"></asp:Label>
    </div>
    <div class="answerOptionContainer">
        <asp:DropDownList ID="dropdownQuestionList" runat="server">
        </asp:DropDownList>
    </div>
