<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StaffSearch.aspx.cs" Inherits="Survey_Prototype.StaffSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="bodyContainer">
        <div class="bodyTitle">
            Staff Search
        </div>

        <div class="searchBoxesContainer">
            <div class="bankCheckBoxes">
                <asp:CheckBoxList  ID="bankCheckBoxList" runat="server">
                    <asp:ListItem Value="17" Text="Westpac" ></asp:ListItem>
                    <asp:ListItem Value="19" Text="ANZ" ></asp:ListItem>
                    <asp:ListItem Value="21" Text="ING"></asp:ListItem>
                </asp:CheckBoxList>
            </div>

            <div class="newspapersCheckBoxList">
                <asp:CheckBoxList ID="bankServiceCheckBoxList" runat="server">
                    <asp:ListItem Value= "48" Text="Internet Banking"></asp:ListItem>
                    <asp:ListItem Value= "49" Text="Home Loan"></asp:ListItem>
                </asp:CheckBoxList>
            </div>

            <div class="genderCheckBoxes">
                <asp:CheckBoxList ID="genderCheckBoxList" runat="server">
                    <asp:ListItem Value="1" Text="Male"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Female"></asp:ListItem>
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
