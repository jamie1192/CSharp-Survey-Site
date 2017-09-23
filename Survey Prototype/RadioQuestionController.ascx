<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RadioQuestionController.ascx.cs" Inherits="Survey_Prototype.RadioQuestionController" %>

    <div class="bodyTitle">
    <asp:Label ID="questionLabel" runat="server" Text="LabelText"></asp:Label>
    </div>
    <div class="answerOptionContainer">
        <asp:RadioButtonList ID="radioQuestionList" runat="server"></asp:RadioButtonList>
    </div>
