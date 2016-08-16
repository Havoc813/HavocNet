<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuCaptureRegatta.aspx.cs" Inherits="HavocNet.Web.Athletica.MenuCaptureRegatta" MasterPageFile="Athletica.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript">
        function AddRace() {
            var tbl = document.getElementById('ctl00_cphMain_tblRaces');

            var aRow = tbl.insertRow(tbl.rows.length - 1);

            var cell0 = aRow.insertCell(0);
            cell0.innerHTML = "Race " + (tbl.rows.length - 1) + " : Placed";
            
            var cell1 = aRow.insertCell(1);
            var txtPlace = document.createElement('input');
            txtPlace.type = 'text';
            txtPlace.className = "PlaceBox";
            txtPlace.attributes["onkeypress"] = "javascript:ChangePlace();";
            txtPlace.id = "txtPlace" + (tbl.rows.length - 1);
            cell1.appendChild(txtPlace);

            var cell2 = aRow.insertCell(2);
            cell2.innerHTML = "of";

            var cell3 = aRow.insertCell(3);
            var txtPlaceParticipants = document.createElement('input');
            txtPlaceParticipants.type = 'text';
            txtPlaceParticipants.className = "PlaceBox";
            txtPlaceParticipants.attributes["onkeypress"] = "javascript:ChangePlaceParticipants();";
            txtPlace.id = "txtParticipants" + (tbl.rows.length - 1);
            cell3.appendChild(txtPlaceParticipants);
        }
        
        function ChangePlace() {
            
        }
        
        function ChangePlaceParticipants() {
            
        }
        
        function OnBeforeSubmit() {
            if (!ValidateDate("ctl00_cphMain_txtDate", "Activity Date")) { return false; }
            if (!ValidateTime("ctl00_cphMain_txtHours", "ctl00_cphMain_txtMinutes", "ctl00_cphMain_txtSeconds")) { return false; }
            if (!ValidateNumber("ctl00_cphMain_txtDistance", "Distance")) { return false; }

            //Validate Race Results
            

            return true;
        }
    </script>
    
    <script src="Scripts/Capture.js"></script>

    <asp:PlaceHolder runat="server" ID="phHiddens"></asp:PlaceHolder>

    <div class="pageTitle">
        <table>
        <tr>
            <td>Record a Regatta</td>
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
                        <tr>
                            <td><div class="CaptureHeader">Location</div></td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="cboLocation" CssClass="Dropdown" />
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
                        <tr><td><div class="CaptureHeader">Results</div></td></tr>
                        <tr>
                            <td>
                                <table id="tblRaces" runat="server">
                                <tr>
                                    <td>Race 1 : Placed</td>
                                    <td><asp:TextBox runat="server" ID="txtPlace1" CssClass="PlaceBox"></asp:TextBox></td>
                                    <td> of </td>
                                    <td><asp:TextBox runat="server" ID="txtPlaceParticipants1" CssClass="PlaceBox"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="padding-top:4px"><a href="javascript:AddRace();" class="MyButton">+</a></td>
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
                                <asp:TextBox ID="txtComment" runat="server" Rows="5" Columns="100" CssClass="CommentBox" TextMode="MultiLine"></asp:TextBox>
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