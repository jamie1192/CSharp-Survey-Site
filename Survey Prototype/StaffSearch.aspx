<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StaffSearch.aspx.cs" Inherits="Survey_Prototype.StaffSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="bodyContainer">
        <div class="bodyTitle">
            Staff Search
        </div>

        <div class="searchBoxesContainer">
            <div class="bankCheckBoxes">
                <asp:CheckBoxList  ID="bankCheckBoxList" runat="server">
                    <asp:ListItem Value="Westpac" Text="Westpac" ></asp:ListItem>
                    <asp:ListItem Value="ANZ" Text="ANZ" ></asp:ListItem>
                    <asp:ListItem Value="ING" Text="ING"></asp:ListItem>
                </asp:CheckBoxList>
            </div>

            <div class="newspapersCheckBoxList">
                <asp:CheckBoxList ID="newspaperCheckBoxList" runat="server">
                    <asp:ListItem Value= "The Daily Telegraph" Text="The Daily Telegraph"></asp:ListItem>
                    <asp:ListItem Value= "The Betoota Advocate" Text="The Betoota Advocate"></asp:ListItem>
                </asp:CheckBoxList>
            </div>

            <div class="genderCheckBoxes">
                <asp:CheckBoxList ID="genderCheckBoxList" runat="server">
                    <asp:ListItem Value="Male" Text="Male"></asp:ListItem>
                    <asp:ListItem Value="Female" Text="Female"></asp:ListItem>
                </asp:CheckBoxList>
            </div>
        </div>
        <asp:Button ID="searchButton" CssClass="staffSearchButton" runat="server" OnClick="Search" Text="Search" />

        
        <div class="gridViewContainer">
            <asp:GridView ID="searchResultsGridView" runat="server" CssClass="GridView" onrowdatabound="SearchResultsGridView_RowDataBound">
            </asp:GridView>
        </div>


    </div>

</asp:Content>
