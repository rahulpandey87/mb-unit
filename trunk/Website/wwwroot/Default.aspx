<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Title="Untitled Page" %>
<asp:Content ID="content" ContentPlaceHolderID="contentPlaceHolder" Runat="Server">
    <h2>Welcome to MbUnit</h2>
    This is the home of the Generative Unit Test Framework for the .NET Framework.&nbsp; MbUnit
    provides advanced using unit testing support with advanced fixtures to enable developers and testers to test all aspects of their software.&nbsp; MbUnit has just released
    version 2.4 and has a dedicated development team working on MbUnit Gallio.<br />
    <br />
    <h3>
        Latest News</h3>
    <asp:Repeater ID="newsItems" runat="server" DataSourceID="SqlDataSource1">
        <ItemTemplate>
            <strong>
                <%# DataBinder.Eval(Container.DataItem, "Title") %>
            </strong>
            <br />
            <%# DataBinder.Eval(Container.DataItem, "Text") %>
            <br />
            <small>Created at
                <%# DataBinder.Eval(Container.DataItem, "Created") %>
                by
                <%# DataBinder.Eval(Container.DataItem, "Author") %>
            </small>
        </ItemTemplate>
        <SeparatorTemplate>
            <br />
            <hr />
        </SeparatorTemplate>
    </asp:Repeater>
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:mbunitDBConnectionString1 %>"
        ProviderName="<%$ ConnectionStrings:mbunitDBConnectionString1.ProviderName %>"
        SelectCommand="SELECT TOP 3 [Title], [Text], [Created], [Author] FROM [NewsItems] ORDER BY [Created] DESC">
    </asp:SqlDataSource>
    
</asp:Content>

