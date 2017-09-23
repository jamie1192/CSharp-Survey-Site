<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextQuestionController.ascx.cs" Inherits="Survey_Prototype.TextQuestionController" %>

    <div class="bodyTitle">
        <asp:Label ID="questionLabel" runat="server" Text="Label"></asp:Label>
    </div>
    <div class="answerOptionContainer">
        <asp:TextBox ID="questionTextBox" runat="server" ></asp:TextBox>
    </div>