<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Question.aspx.cs" Inherits="Survey_Prototype.Question" %>

<asp:Content ID="QuestionContent" ContentPlaceHolderID="MainContent" runat="server">

        <div class="bodyContainer">
            <asp:PlaceHolder ID="QuestionPlaceholder" runat="server">

            </asp:PlaceHolder>
            <div class="buttonContainer">
                    <asp:Button ID="SubmitButton" CssClass="buttonContainerButton" runat="server" OnClick="SubmitButtonClick" Text="Next" />
                </div>
        </div>
 
</asp:Content>
