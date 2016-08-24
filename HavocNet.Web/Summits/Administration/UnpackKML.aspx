<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnpackKML.aspx.cs" Inherits="UnpackKML" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="pageTitleText">
        <table>
        <tr>
            <td>Unpack KML</td>
        </tr>
        </table>
    </div>
    <div style="margin-top: 10px;">
        <table>
            <tr>
                <asp:FileUpload runat="server" ID="upFile"/>
            </tr>
            <tr>
                <td colspan="2" style="padding-top: 8px"><asp:LinkButton runat="server" CssClass="MyButton" Text="Unpack" ID="cmdUnpack" Width="100px"></asp:LinkButton></td>
            </tr>
        </table>
    </div>
</asp:Content>