<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GenerateKML.aspx.cs" Inherits="GenerateKML" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="pageTitleText">
        <table>
        <tr>
            <td>Generate KML</td>
        </tr>
        </table>
    </div>
    <div style="margin-top: 10px;">
        <table>
            <tr>
                <td style="padding:5px">Users</td>
                <td><asp:DropDownList runat="server" ID="cboUsers"/></td>
            </tr>
            <tr>
                <td style="padding:5px">Country</td>
                <td><asp:DropDownList runat="server" ID="cboCountries"/></td>
            </tr>
            <tr>
                <td colspan="2" style="padding-top: 8px"><asp:LinkButton runat="server" CssClass="MyButton" Text="Generate" ID="cmdGenerate" Width="100px"></asp:LinkButton></td>
            </tr>
        </table>
    </div>
</asp:Content>