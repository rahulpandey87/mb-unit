<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="downloads.aspx.cs" Inherits="downloads" Title="Untitled Page" %>
<%@ OutputCache Duration="300" VaryByParam="*" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2><span lang="en-gb">Downloads</span></h2>
<h3><span lang="en-gb">MbUnit 2.3</span></h3>
<p>
    MbUnit 2.3 RTM is now released and available for download.<br />
    <a href="http://prdownload.berlios.de/mbunit/MbUnit-2.3.105.exe">http://prdownload.berlios.de/mbunit/MbUnit-2.3.105.exe</a></p>
    <p>
        Release notes can be found here:<br />
        <a href="http://www.mertner.com/jira/secure/ReleaseNote.jspa?projectId=10020&styleName=Html&version=10150" target="_blank">http://www.mertner.com/jira/secure/ReleaseNote.jspa?projectId=10020&styleName=Html&version=10150</a></p>

    <h3>Build Directory</h3>
    <p>Below are recent development builds. We only recommend these in non-production/critical environments.</p>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 90%">
        <tr>
            <td>
                <asp:Literal ID="releaseOutput" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="downloadOutput" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    <br />

<form runat="server">
    &nbsp;</form>
    <asp:Label ID="errorLabel" Visible="False" runat="server" Text="Could not access build directory" ForeColor="#C00000"></asp:Label>
</asp:Content>

