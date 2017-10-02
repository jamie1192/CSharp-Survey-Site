<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StaffLogin.aspx.cs" Inherits="Survey_Prototype.StaffLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="Stylesheet.css" rel="stylesheet" type="text/css" />

    <div class="bodyContainer">
        <div class="bodyTitle">
            <span class="staffLoginTitle">Staff Login</span>
        </div>

        <div class="loginFields">
            <asp:Label ID="staffUserNameLabel" Text="Username" runat="server"></asp:Label>
            <br />
            <asp:TextBox ID="staffUsernameText" runat="server"></asp:TextBox> <asp:Label ID="loginErrorMessage" runat="server" CssClass="loginErrorMessage"></asp:Label>
            <br />
            <br />
      
            <asp:Label ID="staffPasswordLabel" runat="server" Text="Password"></asp:Label>
            <br />
            <asp:TextBox ID="staffPasswordText" runat="server" TextMode="Password"></asp:TextBox>
            <br />
        </div>

        <div class="loginButtonContainer">
            <asp:Button ID="loginButton" runat="server" Text="Login" OnClick="LoginButton_Click" />
        </div>

    </div>

</asp:Content>
