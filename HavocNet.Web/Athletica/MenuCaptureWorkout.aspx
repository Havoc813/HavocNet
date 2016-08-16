<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuCaptureWorkout.aspx.cs" Inherits="HavocNet.Web.Athletica.MenuCaptureWorkout" MasterPageFile="Athletica.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script src="Scripts/Capture.js"></script>
    <script src="../Scripts/Validate.js"></script>
    
    <script type="text/javascript">
        function OnBeforeSubmit() {
            if (!ValidateDate("ctl00_cphMain_txtDate", "Activity Date")) { return false; }
            if (!ValidateTime("ctl00_cphMain_txtHours", "ctl00_cphMain_txtMinutes", "ctl00_cphMain_txtSeconds")) { return false; }
            if (!ValidateNumber("ctl00_cphMain_txtDistance", "Distance")) { return false; }

            return true;
        }
    </script>

    <asp:PlaceHolder runat="server" ID="phHiddens"></asp:PlaceHolder>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Record a Workout</td>
            <td><asp:ImageButton runat="server" ImageUrl="~/App_Themes/HavocNet/Images/upload.gif" ID="cmdUpload" CssClass="TaskButton" OnClientClick="javascript:return OnBeforeSubmit();"></asp:ImageButton></td>
        </tr>
        </table>
    </div>
    <div>
        <table style="width:100%">
            <tr>
                <td style="width:50%; vertical-align: top">
                    <table>
                        <tr><td><div class="CaptureHeader">Date</div></td></tr>
                        <tr>
                            <td><asp:TextBox runat="server" ID="txtDate" CssClass="DateBox" AutoPostBack="True"></asp:TextBox><asp:DropDownList runat="server" ID="cboTimeOfDay" CssClass="TimeOfDay"/></td>
                        </tr>
                        <tr><td><div class="CaptureHeader">Sport</div></td></tr>
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="cboSport" CssClass="Dropdown"/>
                            </td>
                        </tr>
                        <tr><td><div class="CaptureHeader">Time</div></td></tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td><asp:TextBox runat="server" ID="txtHours" CssClass="TimeTextbox"></asp:TextBox> : </td>
                                        <td><asp:TextBox runat="server" ID="txtMinutes" CssClass="TimeTextbox"></asp:TextBox> : </td>
                                        <td><asp:TextBox runat="server" ID="txtSeconds" CssClass="TimeTextbox"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr><td><div class="CaptureHeader">Route or Distance & Location</div></td></tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="cboRoute" CssClass="Dropdown" onchange="javascript:SetRoute();" /> 
                                            or 
                                            <asp:TextBox runat="server" ID="txtDistance" CssClass="PlaceBox"></asp:TextBox> 
                                            <asp:Label runat="server" ID="lblUnits"></asp:Label>
                                            in 
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="cboLocation" CssClass="TimeOfDay" />
                                        </td>
                                        <td>
                                            <a href="javascript:AddLocation();" class="MyButton">+</a>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNewLocation" CssClass="NewItem"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top; width:50%">
                    <table>
                        <tr><td><div class="CaptureHeader">Kit</div></td></tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="cboKit" CssClass="Dropdown"/>
                                        </td>
                                        <td>
                                            <a href="javascript:AddKit();" class="MyButton">+</a>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNewKit" CssClass="NewItem"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr><td><div class="CaptureHeader">Comments</div></td></tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" Rows="5" Columns="100" CssClass="CommentBox" TextMode="MultiLine" ID="txtComment"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td><div class="CaptureHeader">Tags</div></td></tr>
                        <tr>
                            <td>Tags go here?</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div class="subHeaderText">
    <table>
        <tr>
            <td>Other Activities On This Day</td>
        </tr>
    </table>
    </div>
    <div>
        <asp:PlaceHolder runat="server" ID="phActivities"></asp:PlaceHolder>
    </div>
    <asp:HiddenField runat="server" ID="hidTags" Value="" />
</asp:Content>