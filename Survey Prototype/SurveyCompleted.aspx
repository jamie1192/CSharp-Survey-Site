<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SurveyCompleted.aspx.cs" Inherits="Survey_Prototype.SurveyCompleted" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="Stylesheet.css" rel="stylesheet" type="text/css" />

    <div class="completedSurveyContainer">
        <div class="completedSurveyMessageContainer">
            <img class="successTick" src="Content/check tick.svg" />
            <span class="completedSurveyMessage">Survey completed!</span>
        </div>
    </div>


</asp:Content>
