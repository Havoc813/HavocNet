using System;
using System.Drawing;
using Core.Repositories;
using Phoenix.Core.Servers;

namespace HavocNet.Web
{
    public partial class RetrievePassword : System.Web.UI.Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPageNoAuth("");
            _myMaster.ClearMenus();

            this.cmdReset.Click += cmdReset_click;
        }

        private void cmdReset_click(object sender, EventArgs e)
        {
            var repository = new UserRepository(new HavocServer());

            string newPassword;

            if (repository.TryResetPassword(Request["ctl00$cphMain$txtUsername"], Request["ctl00$cphMain$txtEmailAddress"], out newPassword))
            {
                //TODO: Send an email to the emailaddress provided


                this.txtUsername.Text = "";
                this.txtEmailAddress.Text = "";

                this.lblStatus.Text = @"Password has been reset.  The new password has been sent to the email provided.";
                this.lblStatus.ForeColor = Color.Green;

                this.hidReset.Value = "login";
            }
            else
            {
                this.lblStatus.Text = @"The username and email address combination is incorrect.";
            }
        }
    }
}