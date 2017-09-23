<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckBoxQuestionController.ascx.cs" Inherits="Survey_Prototype.CheckBoxQuestionController" %>

    
        <div class="bodyTitle">
            <asp:Label ID="questionLabel" runat="server" Text="LabelText"></asp:Label>
        </div>
        <div class="answerOptionContainer">
            <asp:CheckBoxList ID="questionCheckBoxList" runat="server">
            </asp:CheckBoxList>
        </div>
    