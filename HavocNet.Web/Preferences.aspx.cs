using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using Core.Repositories;
using Phoenix.Core.Servers;

namespace HavocNet.Web
{
    public partial class Preferences : System.Web.UI.Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("");
            _myMaster.ClearMenus();

            LoadPage();

            this.cmdUpdate.Click += cmdUpdate_click;
        }

        private void LoadPage()
        {
            this.txtFirstName.Text = _myMaster.aUser.FirstName;
            this.txtSurname.Text = _myMaster.aUser.Surname;
            this.txtEmailAddress.Text = _myMaster.aUser.EmailAddress;
            this.chkNewsLetter.Checked = _myMaster.aUser.AllowEmail;

            BindPages();
        }

        private void BindPages()
        {
            this.cboHomePage.Items.Add(new ListItem("Home.aspx", "Home.aspx"));

            this.cboHomePage.Items.FindByValue(_myMaster.aUser.HomePage).Selected = true;
        }

        private void cmdUpdate_click(object sender, ImageClickEventArgs e)
        {
            var aRepository = new UserRepository(new HavocServer());

            Session.Remove("HavocNetUser");
            Session.Add("HavocNetUser", aRepository.UpdatePreferences(
                _myMaster.aUser.ID,
                Request["ctl00$cphMain$txtFirstName"],
                Request["ctl00$cphMain$txtSurname"],
                Request["ctl00$cphMain$txtEmailAddress"],
                Request["ctl00$cphMain$cboHomePage"],
                Request["ctl00$cphMain$chkNewsLetter"] == "on" ? 1 : 0
                                ));

            _myMaster.LoadPage("");
            _myMaster.ClearMenus();

            LoadPage();

            this.lblStatus.Text = @"User Preferences Successfully Updated";
        }
    }
}