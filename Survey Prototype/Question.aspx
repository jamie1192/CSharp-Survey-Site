<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Question.aspx.cs" Inherits="Survey_Prototype.Question" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <asp:PlaceHolder ID="QuestionPlaceholder" runat="server"></asp:PlaceHolder>
        <br />
        <asp:Button ID="SkipButton" runat="server" OnClick="SkipQuestion" Text="Skip Question" />
        <asp:Button ID="SubmitButton" runat="server" OnClick="SubmitButtonClick" Text="Next" />
    </div>

</asp:Content>
